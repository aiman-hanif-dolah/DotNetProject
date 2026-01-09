using DotNetProject.Data;
using DotNetProject.Models;
using Google.Cloud.Firestore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DotNetProject.Services;

public sealed class FirestoreSyncService : BackgroundService
{
    private readonly FirestoreDb _firestoreDb;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<FirestoreSyncService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(10);
    private readonly SemaphoreSlim _syncLock = new(1, 1);

    public FirestoreSyncService(
        FirestoreDb firestoreDb,
        IServiceScopeFactory scopeFactory,
        ILogger<FirestoreSyncService> logger)
    {
        _firestoreDb = firestoreDb;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_interval, stoppingToken);
        }
    }

    public async Task PushAllAsync(CancellationToken ct)
    {
        await _syncLock.WaitAsync(ct);
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await SyncCharactersAsync(dbContext, ct);
            await SyncEpisodesAsync(dbContext, ct);
            await SyncLocationsAsync(dbContext, ct);
            await SyncQuotesAsync(dbContext, ct);
        }
        finally
        {
            _syncLock.Release();
        }
    }

    public async Task PullAllAsync(CancellationToken ct)
    {
        await _syncLock.WaitAsync(ct);
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var characters = await ReadCollectionAsync("characters", ToCharacter, ct);
            var episodes = await ReadCollectionAsync("episodes", ToEpisode, ct);
            var locations = await ReadCollectionAsync("locations", ToLocation, ct);
            var quotes = await ReadCollectionAsync("quotes", ToQuote, ct);

            await ReplaceLocalDataAsync(dbContext, characters, episodes, locations, quotes, ct);
        }
        finally
        {
            _syncLock.Release();
        }
    }

    public Task DeleteAsync(string collection, int id, CancellationToken ct = default)
        => _firestoreDb.Collection(collection)
            .Document(id.ToString())
            .DeleteAsync(cancellationToken: ct);

    private async Task SyncCharactersAsync(AppDbContext dbContext, CancellationToken ct)
    {
        var collection = _firestoreDb.Collection("characters");
        var sqlItems = await dbContext.Characters.IgnoreQueryFilters().ToListAsync(ct);

        foreach (var sql in sqlItems)
        {
            await collection.Document(sql.Id.ToString()).SetAsync(ToFirestore(sql), cancellationToken: ct);
        }
    }

    private async Task SyncEpisodesAsync(AppDbContext dbContext, CancellationToken ct)
    {
        var collection = _firestoreDb.Collection("episodes");
        var sqlItems = await dbContext.Episodes.IgnoreQueryFilters().ToListAsync(ct);

        foreach (var sql in sqlItems)
        {
            await collection.Document(sql.Id.ToString()).SetAsync(ToFirestore(sql), cancellationToken: ct);
        }
    }

    private async Task SyncLocationsAsync(AppDbContext dbContext, CancellationToken ct)
    {
        var collection = _firestoreDb.Collection("locations");
        var sqlItems = await dbContext.Locations.IgnoreQueryFilters().ToListAsync(ct);

        foreach (var sql in sqlItems)
        {
            await collection.Document(sql.Id.ToString()).SetAsync(ToFirestore(sql), cancellationToken: ct);
        }
    }

    private async Task SyncQuotesAsync(AppDbContext dbContext, CancellationToken ct)
    {
        var collection = _firestoreDb.Collection("quotes");
        var sqlItems = await dbContext.Quotes.IgnoreQueryFilters().ToListAsync(ct);

        foreach (var sql in sqlItems)
        {
            await collection.Document(sql.Id.ToString()).SetAsync(ToFirestore(sql), cancellationToken: ct);
        }
    }

    private async Task<List<T>> ReadCollectionAsync<T>(
        string collectionName,
        Func<DocumentSnapshot, T> mapper,
        CancellationToken ct)
    {
        var collection = _firestoreDb.Collection(collectionName);
        var snapshot = await collection.GetSnapshotAsync(ct);
        return snapshot.Documents.Select(mapper).ToList();
    }

    private static Character ToCharacter(DocumentSnapshot doc)
    {
        var data = doc.ToDictionary();
        return new Character
        {
            Id = ParseId(doc.Id, data),
            Name = GetString(data, "Name") ?? string.Empty,
            ActorName = GetString(data, "ActorName") ?? string.Empty,
            Description = GetString(data, "Description"),
            Occupation = GetString(data, "Occupation"),
            ImageUrl = GetString(data, "ImageUrl"),
            VideoUrl = GetString(data, "VideoUrl"),
            UpdatedAtUtc = ParseUpdatedAt(data, doc),
            IsDeleted = GetBool(data, "IsDeleted")
        };
    }

    private static Episode ToEpisode(DocumentSnapshot doc)
    {
        var data = doc.ToDictionary();
        return new Episode
        {
            Id = ParseId(doc.Id, data),
            Title = GetString(data, "Title") ?? string.Empty,
            Season = GetInt(data, "Season"),
            EpisodeNumber = GetInt(data, "EpisodeNumber"),
            AirDate = GetDate(data, "AirDate"),
            Description = GetString(data, "Description"),
            ImageUrl = GetString(data, "ImageUrl"),
            VideoUrl = GetString(data, "VideoUrl"),
            UpdatedAtUtc = ParseUpdatedAt(data, doc),
            IsDeleted = GetBool(data, "IsDeleted")
        };
    }

    private static Location ToLocation(DocumentSnapshot doc)
    {
        var data = doc.ToDictionary();
        return new Location
        {
            Id = ParseId(doc.Id, data),
            Name = GetString(data, "Name") ?? string.Empty,
            Type = GetString(data, "Type") ?? string.Empty,
            Description = GetString(data, "Description"),
            Address = GetString(data, "Address"),
            ImageUrl = GetString(data, "ImageUrl"),
            VideoUrl = GetString(data, "VideoUrl"),
            UpdatedAtUtc = ParseUpdatedAt(data, doc),
            IsDeleted = GetBool(data, "IsDeleted")
        };
    }

    private static Quote ToQuote(DocumentSnapshot doc)
    {
        var data = doc.ToDictionary();
        return new Quote
        {
            Id = ParseId(doc.Id, data),
            Text = GetString(data, "Text") ?? string.Empty,
            Context = GetString(data, "Context"),
            CharacterId = GetInt(data, "CharacterId"),
            EpisodeId = GetInt(data, "EpisodeId"),
            UpdatedAtUtc = ParseUpdatedAt(data, doc),
            IsDeleted = GetBool(data, "IsDeleted")
        };
    }

    private static async Task ReplaceLocalDataAsync(
        AppDbContext dbContext,
        IReadOnlyList<Character> characters,
        IReadOnlyList<Episode> episodes,
        IReadOnlyList<Location> locations,
        IReadOnlyList<Quote> quotes,
        CancellationToken ct)
    {
        await dbContext.Database.OpenConnectionAsync(ct);
        await using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            await ClearTableAsync(dbContext, "Quotes", ct);
            await ClearTableAsync(dbContext, "Locations", ct);
            await ClearTableAsync(dbContext, "Episodes", ct);
            await ClearTableAsync(dbContext, "Characters", ct);

            await InsertWithIdentityAsync(dbContext, "Characters", characters, ct);
            await InsertWithIdentityAsync(dbContext, "Episodes", episodes, ct);
            await InsertWithIdentityAsync(dbContext, "Locations", locations, ct);
            await InsertWithIdentityAsync(dbContext, "Quotes", quotes, ct);

            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
        finally
        {
            await dbContext.Database.CloseConnectionAsync();
        }
    }

    private static Task ClearTableAsync(AppDbContext dbContext, string tableName, CancellationToken ct)
        => dbContext.Database.ExecuteSqlRawAsync($"DELETE FROM [{tableName}]", ct);

    private static async Task InsertWithIdentityAsync<TEntity>(
        AppDbContext dbContext,
        string tableName,
        IReadOnlyList<TEntity> entities,
        CancellationToken ct)
        where TEntity : class
    {
        if (entities.Count == 0)
        {
            return;
        }

        if (dbContext.Database.IsSqlServer())
        {
            await dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT [{tableName}] ON", ct);
            try
            {
                dbContext.Set<TEntity>().AddRange(entities);
                await dbContext.SaveChangesAsync(ct);
            }
            finally
            {
                await dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT [{tableName}] OFF", ct);
            }
        }
        else
        {
            dbContext.Set<TEntity>().AddRange(entities);
            await dbContext.SaveChangesAsync(ct);
        }

        dbContext.ChangeTracker.Clear();
    }

    private static int ParseId(string docId, IReadOnlyDictionary<string, object> data)
        => int.TryParse(docId, out var id) ? id : GetInt(data, "Id");

    private static DateTime ParseUpdatedAt(IReadOnlyDictionary<string, object> data, DocumentSnapshot doc)
    {
        DateTime? fieldValue = null;
        if (data.TryGetValue("UpdatedAtUtc", out var value) && value is Timestamp ts)
        {
            fieldValue = ts.ToDateTime().ToUniversalTime();
        }

        var updateTime = doc.UpdateTime?.ToDateTime().ToUniversalTime();
        if (fieldValue.HasValue && updateTime.HasValue)
        {
            return fieldValue.Value > updateTime.Value ? fieldValue.Value : updateTime.Value;
        }

        return fieldValue ?? updateTime ?? DateTime.MinValue;
    }

    private static string? GetString(IReadOnlyDictionary<string, object> data, string key)
        => data.TryGetValue(key, out var value) ? value as string : null;

    private static int GetInt(IReadOnlyDictionary<string, object> data, string key)
        => data.TryGetValue(key, out var value) && value is long l ? (int)l : 0;

    private static bool GetBool(IReadOnlyDictionary<string, object> data, string key)
        => data.TryGetValue(key, out var value) && value is bool b && b;

    private static DateTime? GetDate(IReadOnlyDictionary<string, object> data, string key)
        => data.TryGetValue(key, out var value) && value is Timestamp ts
            ? ts.ToDateTime().ToUniversalTime()
            : null;

    private static Dictionary<string, object> ToFirestore(Character character)
        => new()
        {
            ["Id"] = character.Id,
            ["Name"] = character.Name,
            ["ActorName"] = character.ActorName,
            ["Description"] = character.Description,
            ["Occupation"] = character.Occupation,
            ["ImageUrl"] = character.ImageUrl,
            ["VideoUrl"] = character.VideoUrl,
            ["UpdatedAtUtc"] = NormalizeUtc(character.UpdatedAtUtc),
            ["IsDeleted"] = character.IsDeleted
        };

    private static Dictionary<string, object> ToFirestore(Episode episode)
        => new()
        {
            ["Id"] = episode.Id,
            ["Title"] = episode.Title,
            ["Season"] = episode.Season,
            ["EpisodeNumber"] = episode.EpisodeNumber,
            ["AirDate"] = NormalizeUtc(episode.AirDate),
            ["Description"] = episode.Description,
            ["ImageUrl"] = episode.ImageUrl,
            ["VideoUrl"] = episode.VideoUrl,
            ["UpdatedAtUtc"] = NormalizeUtc(episode.UpdatedAtUtc),
            ["IsDeleted"] = episode.IsDeleted
        };

    private static Dictionary<string, object> ToFirestore(Location location)
        => new()
        {
            ["Id"] = location.Id,
            ["Name"] = location.Name,
            ["Type"] = location.Type,
            ["Description"] = location.Description,
            ["Address"] = location.Address,
            ["ImageUrl"] = location.ImageUrl,
            ["VideoUrl"] = location.VideoUrl,
            ["UpdatedAtUtc"] = NormalizeUtc(location.UpdatedAtUtc),
            ["IsDeleted"] = location.IsDeleted
        };

    private static Dictionary<string, object> ToFirestore(Quote quote)
        => new()
        {
            ["Id"] = quote.Id,
            ["Text"] = quote.Text,
            ["Context"] = quote.Context,
            ["CharacterId"] = quote.CharacterId,
            ["EpisodeId"] = quote.EpisodeId,
            ["UpdatedAtUtc"] = NormalizeUtc(quote.UpdatedAtUtc),
            ["IsDeleted"] = quote.IsDeleted
        };

    private static bool ShouldPreferSqlMediaUrl(string? sqlUrl, string? firestoreUrl)
        => !string.IsNullOrWhiteSpace(sqlUrl)
           && (IsInvalidFirestoreMediaUrl(firestoreUrl)
               || !string.Equals(sqlUrl.Trim(), firestoreUrl?.Trim(), StringComparison.OrdinalIgnoreCase));

    private static string? NormalizeFirestoreMediaUrl(string? firestoreUrl)
        => IsInvalidFirestoreMediaUrl(firestoreUrl) ? null : firestoreUrl;

    private static bool IsInvalidFirestoreMediaUrl(string? firestoreUrl)
        => string.Equals(
            firestoreUrl?.Trim(),
            "/img/friends/joey.jpg",
            StringComparison.OrdinalIgnoreCase);

    private static DateTime NormalizeUtc(DateTime dateTime)
        => dateTime.Kind == DateTimeKind.Utc ? dateTime : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

    private static DateTime? NormalizeUtc(DateTime? dateTime)
        => dateTime.HasValue ? NormalizeUtc(dateTime.Value) : null;
}
