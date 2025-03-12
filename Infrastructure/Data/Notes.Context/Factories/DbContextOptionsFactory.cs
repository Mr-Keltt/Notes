namespace Notes.Context;

using Microsoft.EntityFrameworkCore;

public static class DbContextOptionsFactory
{
    private const string migrationProjectPrefix = "Notes.Context.Migrations.";

    public static DbContextOptions<MainDbContext> Create(string connStr, DbType dbType, bool detailedLogging = false)
    {
        var bldr = new DbContextOptionsBuilder<MainDbContext>();

        Configure(connStr, dbType, detailedLogging).Invoke(bldr);

        return bldr.Options;
    }

    public static Action<DbContextOptionsBuilder> Configure(string connStr, DbType dbType, bool detailedLogging = false)
    {
        return (bldr) =>
        {
            switch (dbType)
            {
                case DbType.PgSql:
                    bldr.UseNpgsql(connStr,
                        opts => opts
                            .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                            .MigrationsHistoryTable("_migrations")
                            .MigrationsAssembly($"{migrationProjectPrefix}{DbType.PgSql}")
                    );
                    break;
            }

            if (detailedLogging)
            {
                bldr.EnableSensitiveDataLogging();
            }

            bldr.UseLazyLoadingProxies(true);
            //bldr.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
        };
    }
}