using System.Net.Http.Json;
using System.Text.Json;

namespace FriendsFanHub.Maui.Services;

public sealed class FirestoreRestClient
{
    private readonly HttpClient _httpClient;
    private readonly string _projectId;
    private readonly string _apiKey;

    public FirestoreRestClient(HttpClient httpClient, string projectId, string apiKey)
    {
        _httpClient = httpClient;
        _projectId = projectId;
        _apiKey = apiKey;
    }

    public async Task<List<FirestoreDocument>> ListDocumentsAsync(string collection)
    {
        var url = $"{BaseUrl}/{collection}?key={_apiKey}";
        using var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var json = await JsonDocument.ParseAsync(stream);
        if (!json.RootElement.TryGetProperty("documents", out var docs))
        {
            return [];
        }

        var results = new List<FirestoreDocument>();
        foreach (var doc in docs.EnumerateArray())
        {
            var id = doc.GetProperty("name").GetString()?.Split('/').Last() ?? string.Empty;
            results.Add(new FirestoreDocument(id, doc.GetProperty("fields").Clone()));
        }

        return results;
    }

    public async Task<FirestoreDocument?> GetDocumentAsync(string collection, int id)
    {
        var url = $"{BaseUrl}/{collection}/{id}?key={_apiKey}";
        using var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        using var stream = await response.Content.ReadAsStreamAsync();
        using var json = await JsonDocument.ParseAsync(stream);
        var fields = json.RootElement.GetProperty("fields").Clone();
        return new FirestoreDocument(id.ToString(), fields);
    }

    public async Task UpsertDocumentAsync(string collection, int id, Dictionary<string, object?> fields)
    {
        var url = $"{BaseUrl}/{collection}/{id}?key={_apiKey}";
        var payload = new Dictionary<string, object?>
        {
            ["fields"] = FirestoreFieldSerializer.ToFirestoreFields(fields)
        };

        using var response = await _httpClient.PatchAsync(url, JsonContent.Create(payload));
        response.EnsureSuccessStatusCode();
    }

    private string BaseUrl => $"https://firestore.googleapis.com/v1/projects/{_projectId}/databases/(default)/documents";
}

public sealed record FirestoreDocument(string Id, JsonElement Fields);

public static class FirestoreFieldSerializer
{
    public static Dictionary<string, object?> ToFirestoreFields(Dictionary<string, object?> fields)
    {
        var result = new Dictionary<string, object?>();
        foreach (var (key, value) in fields)
        {
            result[key] = ToFirestoreValue(value);
        }

        return result;
    }

    private static Dictionary<string, object?> ToFirestoreValue(object? value)
    {
        if (value is null)
        {
            return new Dictionary<string, object?> { ["nullValue"] = null };
        }

        if (value is DateTime dateTime)
        {
            return new Dictionary<string, object?> { ["timestampValue"] = dateTime.ToUniversalTime().ToString("O") };
        }

        return value switch
        {
            string s => new Dictionary<string, object?> { ["stringValue"] = s },
            int i => new Dictionary<string, object?> { ["integerValue"] = i.ToString() },
            long l => new Dictionary<string, object?> { ["integerValue"] = l.ToString() },
            bool b => new Dictionary<string, object?> { ["booleanValue"] = b },
            _ => new Dictionary<string, object?> { ["stringValue"] = value.ToString() ?? string.Empty }
        };
    }
}
