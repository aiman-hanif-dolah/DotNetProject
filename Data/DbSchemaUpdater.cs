using Microsoft.Data.SqlClient;

namespace DotNetProject.Data;

public static class DbSchemaUpdater
{
    public static void EnsureAuditColumns(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            IF COL_LENGTH('dbo.Characters', 'UpdatedAtUtc') IS NULL
                ALTER TABLE dbo.Characters ADD UpdatedAtUtc datetime2 NOT NULL CONSTRAINT DF_Characters_UpdatedAtUtc DEFAULT SYSUTCDATETIME();
            IF COL_LENGTH('dbo.Characters', 'IsDeleted') IS NULL
                ALTER TABLE dbo.Characters ADD IsDeleted bit NOT NULL CONSTRAINT DF_Characters_IsDeleted DEFAULT 0;

            IF COL_LENGTH('dbo.Episodes', 'UpdatedAtUtc') IS NULL
                ALTER TABLE dbo.Episodes ADD UpdatedAtUtc datetime2 NOT NULL CONSTRAINT DF_Episodes_UpdatedAtUtc DEFAULT SYSUTCDATETIME();
            IF COL_LENGTH('dbo.Episodes', 'IsDeleted') IS NULL
                ALTER TABLE dbo.Episodes ADD IsDeleted bit NOT NULL CONSTRAINT DF_Episodes_IsDeleted DEFAULT 0;

            IF COL_LENGTH('dbo.Locations', 'UpdatedAtUtc') IS NULL
                ALTER TABLE dbo.Locations ADD UpdatedAtUtc datetime2 NOT NULL CONSTRAINT DF_Locations_UpdatedAtUtc DEFAULT SYSUTCDATETIME();
            IF COL_LENGTH('dbo.Locations', 'IsDeleted') IS NULL
                ALTER TABLE dbo.Locations ADD IsDeleted bit NOT NULL CONSTRAINT DF_Locations_IsDeleted DEFAULT 0;

            IF COL_LENGTH('dbo.Quotes', 'UpdatedAtUtc') IS NULL
                ALTER TABLE dbo.Quotes ADD UpdatedAtUtc datetime2 NOT NULL CONSTRAINT DF_Quotes_UpdatedAtUtc DEFAULT SYSUTCDATETIME();
            IF COL_LENGTH('dbo.Quotes', 'IsDeleted') IS NULL
                ALTER TABLE dbo.Quotes ADD IsDeleted bit NOT NULL CONSTRAINT DF_Quotes_IsDeleted DEFAULT 0;
            """;
        command.ExecuteNonQuery();
    }
}
