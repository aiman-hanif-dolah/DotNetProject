using System.Globalization;
using FriendsFanHub.Maui.Helpers;

namespace FriendsFanHub.Maui.Converters;

public class ImagePathConverter : IValueConverter
{
    private const string Fallback = "group.jpg";
    private static readonly Uri FallbackUri = new("https://wallpapers.com/images/hd/friends-tv-show-characters-8t1z1sj8k7wh4i0v.jpg");

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var path = value as string;

        if (string.IsNullOrWhiteSpace(path))
        {
            return ImageSource.FromFile(Fallback);
        }

        // Absolute URL or file path from the DB.
        if (Uri.TryCreate(path, UriKind.Absolute, out var absolute))
        {
            return absolute.IsFile
                ? ImageSource.FromFile(absolute.LocalPath)
                : CreateCachedUriSource(absolute);
        }

        // Relative path (/img/...) should resolve against the MVC host.
        if (path.StartsWith("/", StringComparison.Ordinal))
        {
            var backendUri = AppConfig.BuildBackendUri(path);
            if (backendUri != null)
            {
                return CreateCachedUriSource(backendUri);
            }
        }

        // Try bundled assets that mirror the MVC naming.
        var localFromName = LoadFromPackage(SanitizeFileName(Path.GetFileName(path)));
        if (localFromName != null)
        {
            return localFromName;
        }

        // Finally, attempt direct file/uri resolution before falling back.
        var direct = TryLoadUriOrFile(path);
        if (direct != null)
        {
            return direct;
        }

        return ImageSource.FromUri(FallbackUri);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();

    private static string SanitizeFileName(string fileName) =>
        fileName.Replace("-", "_");

    private static ImageSource CreateCachedUriSource(Uri uri) =>
        new UriImageSource
        {
            Uri = uri,
            CachingEnabled = true,
            CacheValidity = TimeSpan.FromDays(7)
        };

    private static ImageSource? LoadFromPackage(string logicalName)
    {
        if (string.IsNullOrWhiteSpace(logicalName))
        {
            return null;
        }

        try
        {
            return ImageSource.FromFile(logicalName);
        }
        catch
        {
            return null;
        }
    }

    private static ImageSource? TryLoadUriOrFile(string path)
    {
        if (Uri.TryCreate(path, UriKind.Absolute, out var uri))
        {
            if (uri.IsFile)
            {
                return ImageSource.FromFile(uri.LocalPath);
            }

            return CreateCachedUriSource(uri);
        }

        try
        {
            return ImageSource.FromFile(path);
        }
        catch
        {
            return null;
        }
    }
}
