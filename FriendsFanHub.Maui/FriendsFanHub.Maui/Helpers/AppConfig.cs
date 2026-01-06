using Microsoft.Maui.Storage;

namespace FriendsFanHub.Maui.Helpers;

/// <summary>
/// Central place for runtime configuration shared across the app.
/// BackendBaseUrl should point to the MVC site so relative media paths resolve.
/// </summary>
public static class AppConfig
{
    // Default to Android emulator loopback hitting the MVC http profile.
    public const string DefaultBackendBaseUrl = "http://10.0.2.2:5263";
    public const string DefaultFirestoreProjectId = "friends-fan-hub";
    public const string DefaultFirestoreApiKey = "YOUR_FIREBASE_WEB_API_KEY";

    public static string BackendBaseUrl =>
        Preferences.Get(nameof(BackendBaseUrl), DefaultBackendBaseUrl);

    public static string FirestoreProjectId =>
        Preferences.Get(nameof(FirestoreProjectId), DefaultFirestoreProjectId);

    public static string FirestoreApiKey =>
        Preferences.Get(nameof(FirestoreApiKey), DefaultFirestoreApiKey);

    public static Uri? BuildBackendUri(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return null;
        }

        var baseUrl = BackendBaseUrl?.TrimEnd('/');
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return null;
        }

        var trimmed = relativePath.TrimStart('/');
        return Uri.TryCreate($"{baseUrl}/{trimmed}", UriKind.Absolute, out var uri)
            ? uri
            : null;
    }
}
