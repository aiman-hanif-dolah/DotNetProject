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
            try
            {
                await SyncAllAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Firestore sync failed.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task SyncAllAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await SyncCharactersAsync(dbContext, ct);
        await SyncEpisodesAsync(dbContext, ct);
        await SyncLocationsAsync(dbContext, ct);
        await SyncQuotesAsync(dbContext, ct);
    }

    private async Task SyncCharactersAsync(AppDbContext dbContext, CancellationToken ct)
    {
        var collection = _firestoreDb.Collection("characters");
        var snapshot = await collection.GetSnapshotAsync(ct);
        var firestoreDocs = snapshot.Documents.ToDictionary(d => d.Id, d => d);

        var sqlItems = await dbContext.Characters.IgnoreQueryFilters().ToListAsync(ct);
        var sqlById = sqlItems.ToDictionary(c => c.Id);

        foreach (var doc in snapshot.Documents)
        {
            var data = doc.ToDictionary();
            var id = ParseId(doc.Id, data);
            var updatedAt = ParseUpdatedAt(data, doc);
            var isDeleted = GetBool(data, "IsDeleted");

            if (!sqlById.TryGetValue(id, out var entity))
            {
                dbContext.Characters.Add(new Character
                {
                    Id = id,
                    Name = GetString(data, "Name") ?? string.Empty,
                    ActorName = GetString(data, "ActorName") ?? string.Empty,
                    Description = GetString(data, "Description"),
                    Occupation = GetString(data, "Occupation"),
                    ImageUrl = GetString(data, "ImageUrl"),
                    VideoUrl = GetString(data, "VideoUrl"),
                    UpdatedAtUtc = updatedAt,
                    IsDeleted = isDeleted
                });
            }
            else if (updatedAt >= entity.UpdatedAtUtc)
            {
                entity.Name = GetString(data, "Name") ?? entity.Name;
                entity.ActorName = GetString(data, "ActorName") ?? entity.ActorName;
                entity.Description = GetString(data, "Description");
                entity.Occupation = GetString(data, "Occupation");
                entity.ImageUrl = GetString(data, "ImageUrl");
                entity.VideoUrl = GetString(data, "VideoUrl");
                entity.UpdatedAtUtc = updatedAt;
                entity.IsDeleted = isDeleted;
            }
        }

        await dbContext.SaveChangesAsync(ct);

        foreach (var sql in sqlItems)
        {
            if (!firestoreDocs.TryGetValue(sql.Id.ToString(), out var doc))
            {
                await collection.Document(sql.Id.ToString()).SetAsync(ToFirestore(sql), cancellationToken: ct);
                continue;
            }

            var data = doc.ToDictionary();
            var updatedAt = ParseUpdatedAt(data, doc);
            if (sql.UpdatedAtUtc > updatedAt)
            {
                await collection.Document(sql.Id.ToString()).SetAsync(ToFirestore(sql), cancellationToken: ct);
            }
        }
    }

    private async Task SyncEpisodesAsync(AppDbContext dbContext, CancellationToken ct)
    {
        var collection = _firestoreDb.Collection("episodes");
        var snapshot = await collection.GetSnapshotAsync(ct);
        var firestoreDocs = snapshot.Documents.ToDictionary(d => d.Id, d => d);

        var sqlItems = await dbContext.Episodes.IgnoreQueryFilters().ToListAsync(ct);
        var sqlById = sqlItems.ToDictionary(c => c.Id);

        foreach (var doc in snapshot.Documents)
        {
            var data = doc.ToDictionary();
            var id = ParseId(doc.Id, data);
            var updatedAt = ParseUpdatedAt(data, doc);
            var isDeleted = GetBool(data, "IsDeleted");

            if (!sqlById.TryGetValue(id, out var entity))
            {
                dbContext.Episodes.Add(new Episode
                {
                    Id = id,
                    Title = GetString(data, "Title") ?? string.Empty,
                    Season = GetInt(data, "Season"),
                    EpisodeNumber = GetInt(data, "EpisodeNumber"),
                    AirDate = GetDate(data, "AirDate"),
                    Description = GetString(data, "Description"),
                    ImageUrl = GetString(data, "ImageUrl"),
                    VideoUrl = GetString(data, "VideoUrl"),
                    UpdatedAtUtc = updatedAt,
                    IsDeleted = isDeleted
                });
            }
            else if (updatedAt >= entity.UpdatedAtUtc)
            {
                entity.Title = GetString(data, "Title") ?? entity.Title;
                entity.Season = GetInt(data, "Season");
                entity.EpisodeNumber = GetInt(data, "EpisodeNumber");
                entity.AirDate = GetDate(data, "AirDate");
                entity.Description = GetString(data, "Description");
                entity.ImageUrl = GetString(data, "ImageUrl");
                entity.VideoUrl = GetString(data, "VideoUrl");
                entity.UpdatedAtUtc = updatedAt;
                entity.IsDeleted = isDeleted;
            }
        }

        await dbContext.SaveChangesAsync(ct);

        foreach (var sql in sqlItems)
        {
            if (!firestoreDocs.TryGetValue(sql.Id.ToString(), out var doc))
            {
                await collection.Document(sql.Id.ToString()).SetAsync(ToFirestore(sql), cancellationToken: ct);
                continue;
            }

            var data = doc.ToDictionary();
            var updatedAt = ParseUpdatedAt(data, doc);
            if (sql.UpdatedAtUtc > updatedAt)
            {
                await collection.Document(sql.Id.ToString()).SetAsync(ToFirestore(sql), cancellationToken: ct);
            }
        }
    }

    private async Task SyncLocationsAsync(AppDbContext dbContext, CancellationToken ct)
    {
        var collection = _firestoreDb.Collection("locations");
        var snapshot = await collection.GetSnapshotAsync(ct);
        var firestoreDocs = snapshot.Documents.ToDictionary(d => d.Id, d => d);

        var sqlItems = await dbContext.Locations.IgnoreQueryFilters().ToListAsync(ct);
        var sqlById = sqlItems.ToDictionary(c => c.Id);

        foreach (var doc in snapshot.Documents)
        {
            var data = doc.ToDictionary();
            var id = ParseId(doc.Id, data);
            var updatedAt = ParseUpdatedAt(data, doc);
            var isDeleted = GetBool(data, "IsDeleted");

            if (!sqlById.TryGetValue(id, out var entity))
            {
                dbContext.Locations.Add(new Location
                {
                    Id = id,
                    Name = GetString(data, "Name") ?? string.Empty,
                    Type = GetString(data, "Type") ?? string.Empty,
                    Description = GetString(data, "Description"),
                    Address = GetString(data, "Address"),
                    ImageUrl = GetString(data, "ImageUrl"),
                    VideoUrl = GetString(data, "VideoUrl"),
                    UpdatedAtUtc = updatedAt,
                    IsDeleted = isDeleted
                });
            }
            else if (updatedAt >= entity.UpdatedAtUtc)
            {
                entity.Name = GetString(data, "Name") ?? entity.Name;
                entity.Type = GetString(data, "Type") ?? entity.Type;
                entity.Description = GetString(data, "Description");
                entity.Address = GetString(data, "Address");
                entity.ImageUrl = GetString(data, "ImageUrl");
                entity.VideoUrl = GetString(data, "VideoUrl");
                entity.UpdatedAtUtc = updatedAt;
                entity.IsDeleted = isDeleted;
            }
        }

        await dbContext.SaveChangesAsync(ct);

        foreach (var sql in sqlItems)
        {
            if (!firestoreDocs.TryGetValue(sql.Id.ToString(), out var doc))
            {
                await collection.Document(sql.Id.ToString()).SetAsync(ToFirestore(sql), cancellationToken: ct);
                continue;
            }

            var data = doc.ToDictionary();
            var updatedAt = ParseUpdatedAt(data, doc);
            if (sql.UpdatedAtUtc > updatedAt)
            {
                await collection.Document(sql.Id.ToString()).SetAsync(ToFirestore(sql), cancellationToken: ct);
            }
        }
    }

    private async Task SyncQuotesAsync(AppDbContext dbContext, CancellationToken ct)
    {
        var collection = _firestoreDb.Collection("quotes");
        var snapshot = await collection.GetSnapshotAsync(ct);
        var firestoreDocs = snapshot.Documents.ToDictionary(d => d.Id, d => d);

        var sqlItems = await dbContext.Quotes.IgnoreQueryFilters().ToListAsync(ct);
        var sqlById = sqlItems.ToDictionary(c => c.Id);

        foreach (var doc in snapshot.Documents)
        {
            var data = doc.ToDictionary();
            var id = ParseId(doc.Id, data);
            var updatedAt = ParseUpdatedAt(data, doc);
            var isDeleted = GetBool(data, "IsDeleted");

            if (!sqlById.TryGetValue(id, out var entity))
            {
                dbContext.Quotes.Add(new Quote
                {
                    Id = id,
                    Text = GetString(data, "Text") ?? string.Empty,
                    Context = GetString(data, "Context"),
                    CharacterId = GetInt(data, "CharacterId"),
                    EpisodeId = GetInt(data, "EpisodeId"),
                    UpdatedAtUtc = updatedAt,
                    IsDeleted = isDeleted
                });
            }
            else if (updatedAt >= entity.UpdatedAtUtc)
            {
                entity.Text = GetString(data, "Text") ?? entity.Text;
                entity.Context = GetString(data, "Context");
                entity.CharacterId = GetInt(data, "CharacterId");
                entity.EpisodeId = GetInt(data, "EpisodeId");
                entity.UpdatedAtUtc = updatedAt;
                entity.IsDeleted = isDeleted;
            }
        }

        await dbContext.SaveChangesAsync(ct);

        foreach (var sql in sqlItems)
        {
            if (!firestoreDocs.TryGetValue(sql.Id.ToString(), out var doc))
            {
                await collection.Document(sql.Id.ToString()).SetAsync(ToFirestore(sql), cancellationToken: ct);
                continue;
            }

            var data = doc.ToDictionary();
            var updatedAt = ParseUpdatedAt(data, doc);
            if (sql.UpdatedAtUtc > updatedAt)
            {
                await collection.Document(sql.Id.ToString()).SetAsync(ToFirestore(sql), cancellationToken: ct);
            }
        }
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

    private static DateTime NormalizeUtc(DateTime dateTime)
        => dateTime.Kind == DateTimeKind.Utc ? dateTime : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

    private static DateTime? NormalizeUtc(DateTime? dateTime)
        => dateTime.HasValue ? NormalizeUtc(dateTime.Value) : null;
}
