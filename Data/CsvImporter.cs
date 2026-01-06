using System.Globalization;
using DotNetProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;

namespace DotNetProject.Data;

public static class CsvImporter
{
    public static void ImportAll(AppDbContext dbContext, string contentRootPath)
    {
        var dataDir = Path.Combine(contentRootPath, "Data");
        var now = DateTime.UtcNow;
        var characters = ReadCsv(Path.Combine(dataDir, "Character.csv"), fields => new Character
        {
            Id = ParseInt(fields, 0),
            Name = ParseString(fields, 1),
            ActorName = ParseString(fields, 2),
            Description = ParseNullable(fields, 3),
            Occupation = ParseNullable(fields, 4),
            ImageUrl = ParseNullable(fields, 5),
            VideoUrl = ParseNullable(fields, 6),
            UpdatedAtUtc = now,
            IsDeleted = false
        });

        var episodes = ReadCsv(Path.Combine(dataDir, "Episode.csv"), fields => new Episode
        {
            Id = ParseInt(fields, 0),
            Title = ParseString(fields, 1),
            Season = ParseInt(fields, 2),
            EpisodeNumber = ParseInt(fields, 3),
            AirDate = ParseDate(fields, 4),
            Description = ParseNullable(fields, 5),
            ImageUrl = ParseNullable(fields, 6),
            VideoUrl = ParseNullable(fields, 7),
            UpdatedAtUtc = now,
            IsDeleted = false
        });

        var locations = ReadCsv(Path.Combine(dataDir, "Location.csv"), fields => new Location
        {
            Id = ParseInt(fields, 0),
            Name = ParseString(fields, 1),
            Type = ParseString(fields, 2),
            Description = ParseNullable(fields, 3),
            Address = ParseNullable(fields, 4),
            ImageUrl = ParseNullable(fields, 5),
            VideoUrl = ParseNullable(fields, 6),
            UpdatedAtUtc = now,
            IsDeleted = false
        });

        var quotes = ReadCsv(Path.Combine(dataDir, "Quotes.csv"), fields => new Quote
        {
            Id = ParseInt(fields, 0),
            Text = ParseString(fields, 1),
            Context = ParseNullable(fields, 2),
            CharacterId = ParseInt(fields, 3),
            EpisodeId = ParseInt(fields, 4),
            UpdatedAtUtc = now,
            IsDeleted = false
        });

        using var transaction = dbContext.Database.BeginTransaction();
        dbContext.Database.OpenConnection();
        try
        {
            dbContext.Database.ExecuteSqlRaw("DELETE FROM Quotes");
            dbContext.Database.ExecuteSqlRaw("DELETE FROM Characters");
            dbContext.Database.ExecuteSqlRaw("DELETE FROM Episodes");
            dbContext.Database.ExecuteSqlRaw("DELETE FROM Locations");

            InsertWithIdentity(dbContext, "Characters", () => dbContext.Characters.AddRange(characters));
            InsertWithIdentity(dbContext, "Episodes", () => dbContext.Episodes.AddRange(episodes));
            InsertWithIdentity(dbContext, "Locations", () => dbContext.Locations.AddRange(locations));
            InsertWithIdentity(dbContext, "Quotes", () => dbContext.Quotes.AddRange(quotes));

            transaction.Commit();
        }
        finally
        {
            dbContext.Database.CloseConnection();
        }
    }

    private static List<T> ReadCsv<T>(string path, Func<string[], T> map)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"CSV not found: {path}");
        }

        var rows = new List<T>();
        using var parser = new TextFieldParser(path)
        {
            TextFieldType = FieldType.Delimited,
            HasFieldsEnclosedInQuotes = true
        };
        parser.SetDelimiters(",");

        while (!parser.EndOfData)
        {
            var fields = parser.ReadFields();
            if (fields is null || fields.Length == 0)
            {
                continue;
            }

            if (rows.Count == 0 && !int.TryParse(fields[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
            {
                continue;
            }

            rows.Add(map(fields));
        }

        return rows;
    }

    private static void InsertWithIdentity(AppDbContext dbContext, string tableName, Action addEntities)
    {
        dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {tableName} ON");
        addEntities();
        dbContext.SaveChanges();
        dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {tableName} OFF");
    }

    private static string ParseString(string[] fields, int index)
        => fields.Length > index ? fields[index] : string.Empty;

    private static string? ParseNullable(string[] fields, int index)
    {
        if (fields.Length <= index)
        {
            return null;
        }

        var value = fields[index];
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    private static int ParseInt(string[] fields, int index)
        => int.TryParse(ParseString(fields, index), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
            ? value
            : 0;

    private static DateTime? ParseDate(string[] fields, int index)
        => DateTime.TryParse(ParseString(fields, index), CultureInfo.InvariantCulture, DateTimeStyles.None, out var value)
            ? value
            : null;
}
