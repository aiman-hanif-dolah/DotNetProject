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

    public static string BackendBaseUrl =>
        Preferences.Get(nameof(BackendBaseUrl), DefaultBackendBaseUrl);

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
