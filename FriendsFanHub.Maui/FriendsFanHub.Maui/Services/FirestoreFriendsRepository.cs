using System.Text.Json;
using FriendsFanHub.Maui.Helpers;
using FriendsFanHub.Maui.Models;
using LocationModel = FriendsFanHub.Maui.Models.Location;

namespace FriendsFanHub.Maui.Services;

public sealed class FirestoreFriendsRepository : IFriendsRepository
{
    private readonly FirestoreRestClient _client;

    public FirestoreFriendsRepository(FirestoreRestClient client)
    {
        _client = client;
    }

    public async Task<List<Character>> GetCharactersAsync()
        => (await _client.ListDocumentsAsync("characters"))
            .Select(MapCharacter)
            .Where(c => !c.IsDeleted)
            .ToList();

    public async Task<Character?> GetCharacterAsync(int id)
        => await MapSingleAsync("characters", id, MapCharacter);

    public async Task<Character> AddCharacterAsync(Character character)
    {
        character.Id = await GetNextIdAsync("characters");
        character.UpdatedAtUtc = DateTime.UtcNow;
        character.IsDeleted = false;
        await _client.UpsertDocumentAsync("characters", character.Id, ToFields(character));
        return character;
    }

    public async Task UpdateCharacterAsync(Character character)
    {
        character.UpdatedAtUtc = DateTime.UtcNow;
        await _client.UpsertDocumentAsync("characters", character.Id, ToFields(character));
    }

    public async Task DeleteCharacterAsync(int id)
        => await SoftDeleteAsync("characters", id);

    public async Task<List<Episode>> GetEpisodesAsync()
        => (await _client.ListDocumentsAsync("episodes"))
            .Select(MapEpisode)
            .Where(e => !e.IsDeleted)
            .ToList();

    public async Task<Episode?> GetEpisodeAsync(int id)
        => await MapSingleAsync("episodes", id, MapEpisode);

    public async Task<Episode> AddEpisodeAsync(Episode episode)
    {
        episode.Id = await GetNextIdAsync("episodes");
        episode.UpdatedAtUtc = DateTime.UtcNow;
        episode.IsDeleted = false;
        await _client.UpsertDocumentAsync("episodes", episode.Id, ToFields(episode));
        return episode;
    }

    public async Task UpdateEpisodeAsync(Episode episode)
    {
        episode.UpdatedAtUtc = DateTime.UtcNow;
        await _client.UpsertDocumentAsync("episodes", episode.Id, ToFields(episode));
    }

    public async Task DeleteEpisodeAsync(int id)
        => await SoftDeleteAsync("episodes", id);

    public async Task<List<LocationModel>> GetLocationsAsync()
        => (await _client.ListDocumentsAsync("locations"))
            .Select(MapLocation)
            .Where(l => !l.IsDeleted)
            .ToList();

    public async Task<LocationModel?> GetLocationAsync(int id)
        => await MapSingleAsync("locations", id, MapLocation);

    public async Task<LocationModel> AddLocationAsync(LocationModel location)
    {
        location.Id = await GetNextIdAsync("locations");
        location.UpdatedAtUtc = DateTime.UtcNow;
        location.IsDeleted = false;
        await _client.UpsertDocumentAsync("locations", location.Id, ToFields(location));
        return location;
    }

    public async Task UpdateLocationAsync(LocationModel location)
    {
        location.UpdatedAtUtc = DateTime.UtcNow;
        await _client.UpsertDocumentAsync("locations", location.Id, ToFields(location));
    }

    public async Task DeleteLocationAsync(int id)
        => await SoftDeleteAsync("locations", id);

    public async Task<List<Quote>> GetQuotesAsync()
        => (await _client.ListDocumentsAsync("quotes"))
            .Select(MapQuote)
            .Where(q => !q.IsDeleted)
            .ToList();

    public async Task<Quote?> GetQuoteAsync(int id)
        => await MapSingleAsync("quotes", id, MapQuote);

    public async Task<Quote> AddQuoteAsync(Quote quote)
    {
        quote.Id = await GetNextIdAsync("quotes");
        quote.UpdatedAtUtc = DateTime.UtcNow;
        quote.IsDeleted = false;
        await _client.UpsertDocumentAsync("quotes", quote.Id, ToFields(quote));
        return quote;
    }

    public async Task UpdateQuoteAsync(Quote quote)
    {
        quote.UpdatedAtUtc = DateTime.UtcNow;
        await _client.UpsertDocumentAsync("quotes", quote.Id, ToFields(quote));
    }

    public async Task DeleteQuoteAsync(int id)
        => await SoftDeleteAsync("quotes", id);

    public async Task<DashboardSnapshot> GetDashboardAsync()
    {
        var characters = await GetCharactersAsync();
        var episodes = await GetEpisodesAsync();
        var locations = await GetLocationsAsync();
        var quotes = await GetQuotesAsync();

        return new DashboardSnapshot
        {
            CharacterCount = characters.Count,
            EpisodeCount = episodes.Count,
            LocationCount = locations.Count,
            QuoteCount = quotes.Count
        };
    }

    private async Task<T?> MapSingleAsync<T>(string collection, int id, Func<FirestoreDocument, T> map)
    {
        var doc = await _client.GetDocumentAsync(collection, id);
        if (doc is null)
        {
            return default;
        }

        var mapped = map(doc);
        if (mapped is Character c && c.IsDeleted) return default;
        if (mapped is Episode e && e.IsDeleted) return default;
        if (mapped is LocationModel l && l.IsDeleted) return default;
        if (mapped is Quote q && q.IsDeleted) return default;
        return mapped;
    }

    private async Task SoftDeleteAsync(string collection, int id)
    {
        var existing = await _client.GetDocumentAsync(collection, id);
        if (existing is null)
        {
            return;
        }

        if (collection == "characters")
        {
            var entity = MapCharacter(existing);
            entity.IsDeleted = true;
            entity.UpdatedAtUtc = DateTime.UtcNow;
            await _client.UpsertDocumentAsync(collection, id, ToFields(entity));
            return;
        }

        if (collection == "episodes")
        {
            var entity = MapEpisode(existing);
            entity.IsDeleted = true;
            entity.UpdatedAtUtc = DateTime.UtcNow;
            await _client.UpsertDocumentAsync(collection, id, ToFields(entity));
            return;
        }

        if (collection == "locations")
        {
            var entity = MapLocation(existing);
            entity.IsDeleted = true;
            entity.UpdatedAtUtc = DateTime.UtcNow;
            await _client.UpsertDocumentAsync(collection, id, ToFields(entity));
            return;
        }

        if (collection == "quotes")
        {
            var entity = MapQuote(existing);
            entity.IsDeleted = true;
            entity.UpdatedAtUtc = DateTime.UtcNow;
            await _client.UpsertDocumentAsync(collection, id, ToFields(entity));
        }
    }

    private async Task<int> GetNextIdAsync(string collection)
    {
        var docs = await _client.ListDocumentsAsync(collection);
        var max = 0;
        foreach (var doc in docs)
        {
            var id = ParseInt(doc.Fields, "Id");
            if (id == 0 && int.TryParse(doc.Id, out var parsed))
            {
                id = parsed;
            }

            if (id > max)
            {
                max = id;
            }
        }

        return max + 1;
    }

    private static Character MapCharacter(FirestoreDocument doc)
        => new()
        {
            Id = ParseInt(doc.Fields, "Id", doc.Id),
            Name = ParseString(doc.Fields, "Name") ?? string.Empty,
            ActorName = ParseString(doc.Fields, "ActorName") ?? string.Empty,
            Description = ParseString(doc.Fields, "Description"),
            Occupation = ParseString(doc.Fields, "Occupation"),
            ImageUrl = ParseString(doc.Fields, "ImageUrl"),
            VideoUrl = ParseString(doc.Fields, "VideoUrl"),
            UpdatedAtUtc = ParseDate(doc.Fields, "UpdatedAtUtc") ?? DateTime.UtcNow,
            IsDeleted = ParseBool(doc.Fields, "IsDeleted")
        };

    private static Episode MapEpisode(FirestoreDocument doc)
        => new()
        {
            Id = ParseInt(doc.Fields, "Id", doc.Id),
            Title = ParseString(doc.Fields, "Title") ?? string.Empty,
            Season = ParseInt(doc.Fields, "Season"),
            EpisodeNumber = ParseInt(doc.Fields, "EpisodeNumber"),
            AirDate = ParseDate(doc.Fields, "AirDate"),
            Description = ParseString(doc.Fields, "Description"),
            ImageUrl = ParseString(doc.Fields, "ImageUrl"),
            VideoUrl = ParseString(doc.Fields, "VideoUrl"),
            UpdatedAtUtc = ParseDate(doc.Fields, "UpdatedAtUtc") ?? DateTime.UtcNow,
            IsDeleted = ParseBool(doc.Fields, "IsDeleted")
        };

    private static LocationModel MapLocation(FirestoreDocument doc)
        => new()
        {
            Id = ParseInt(doc.Fields, "Id", doc.Id),
            Name = ParseString(doc.Fields, "Name") ?? string.Empty,
            Type = ParseString(doc.Fields, "Type") ?? string.Empty,
            Description = ParseString(doc.Fields, "Description"),
            Address = ParseString(doc.Fields, "Address"),
            ImageUrl = ParseString(doc.Fields, "ImageUrl"),
            VideoUrl = ParseString(doc.Fields, "VideoUrl"),
            UpdatedAtUtc = ParseDate(doc.Fields, "UpdatedAtUtc") ?? DateTime.UtcNow,
            IsDeleted = ParseBool(doc.Fields, "IsDeleted")
        };

    private static Quote MapQuote(FirestoreDocument doc)
        => new()
        {
            Id = ParseInt(doc.Fields, "Id", doc.Id),
            Text = ParseString(doc.Fields, "Text") ?? string.Empty,
            Context = ParseString(doc.Fields, "Context"),
            CharacterId = ParseInt(doc.Fields, "CharacterId"),
            EpisodeId = ParseInt(doc.Fields, "EpisodeId"),
            UpdatedAtUtc = ParseDate(doc.Fields, "UpdatedAtUtc") ?? DateTime.UtcNow,
            IsDeleted = ParseBool(doc.Fields, "IsDeleted")
        };

    private static Dictionary<string, object?> ToFields(Character character)
        => new()
        {
            ["Id"] = character.Id,
            ["Name"] = character.Name,
            ["ActorName"] = character.ActorName,
            ["Description"] = character.Description,
            ["Occupation"] = character.Occupation,
            ["ImageUrl"] = character.ImageUrl,
            ["VideoUrl"] = character.VideoUrl,
            ["UpdatedAtUtc"] = character.UpdatedAtUtc,
            ["IsDeleted"] = character.IsDeleted
        };

    private static Dictionary<string, object?> ToFields(Episode episode)
        => new()
        {
            ["Id"] = episode.Id,
            ["Title"] = episode.Title,
            ["Season"] = episode.Season,
            ["EpisodeNumber"] = episode.EpisodeNumber,
            ["AirDate"] = episode.AirDate,
            ["Description"] = episode.Description,
            ["ImageUrl"] = episode.ImageUrl,
            ["VideoUrl"] = episode.VideoUrl,
            ["UpdatedAtUtc"] = episode.UpdatedAtUtc,
            ["IsDeleted"] = episode.IsDeleted
        };

    private static Dictionary<string, object?> ToFields(LocationModel location)
        => new()
        {
            ["Id"] = location.Id,
            ["Name"] = location.Name,
            ["Type"] = location.Type,
            ["Description"] = location.Description,
            ["Address"] = location.Address,
            ["ImageUrl"] = location.ImageUrl,
            ["VideoUrl"] = location.VideoUrl,
            ["UpdatedAtUtc"] = location.UpdatedAtUtc,
            ["IsDeleted"] = location.IsDeleted
        };

    private static Dictionary<string, object?> ToFields(Quote quote)
        => new()
        {
            ["Id"] = quote.Id,
            ["Text"] = quote.Text,
            ["Context"] = quote.Context,
            ["CharacterId"] = quote.CharacterId,
            ["EpisodeId"] = quote.EpisodeId,
            ["UpdatedAtUtc"] = quote.UpdatedAtUtc,
            ["IsDeleted"] = quote.IsDeleted
        };

    private static string? ParseString(JsonElement fields, string name)
        => fields.TryGetProperty(name, out var field) && field.TryGetProperty("stringValue", out var value)
            ? value.GetString()
            : null;

    private static int ParseInt(JsonElement fields, string name, string? fallback = null)
    {
        if (fields.TryGetProperty(name, out var field) && field.TryGetProperty("integerValue", out var value))
        {
            return int.TryParse(value.GetString(), out var parsed) ? parsed : 0;
        }

        return int.TryParse(fallback, out var fallbackId) ? fallbackId : 0;
    }

    private static bool ParseBool(JsonElement fields, string name)
        => fields.TryGetProperty(name, out var field) && field.TryGetProperty("booleanValue", out var value)
            && value.GetBoolean();

    private static DateTime? ParseDate(JsonElement fields, string name)
        => fields.TryGetProperty(name, out var field) && field.TryGetProperty("timestampValue", out var value)
            ? DateTime.TryParse(value.GetString(), out var parsed) ? parsed.ToUniversalTime() : null
            : null;
}
