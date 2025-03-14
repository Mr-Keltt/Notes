namespace Notes.Context.Factories
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Provides factory methods for creating and configuring DbContextOptions for MainDbContext.
    /// </summary>
    public static class DbContextOptionsFactory
    {
        // Prefix used for identifying the migrations assembly for different database types.
        private const string migrationProjectPrefix = "Notes.Context.Migrations.";

        /// <summary>
        /// Creates and returns a new instance of DbContextOptions for MainDbContext using the specified connection string, database type, and logging settings.
        /// </summary>
        /// <param name="connStr">The connection string to the database.</param>
        /// <param name="dbType">The type of the database (e.g., PgSql).</param>
        /// <param name="detailedLogging">If set to <c>true</c>, enables detailed logging including sensitive data.</param>
        /// <returns>A configured instance of DbContextOptions for MainDbContext.</returns>
        public static DbContextOptions<MainDbContext> Create(string connStr, DbType dbType, bool detailedLogging = false)
        {
            // Initialize a new DbContextOptionsBuilder for MainDbContext.
            var bldr = new DbContextOptionsBuilder<MainDbContext>();

            // Apply the configuration to the builder.
            Configure(connStr, dbType, detailedLogging).Invoke(bldr);

            // Return the constructed options.
            return bldr.Options;
        }

        /// <summary>
        /// Returns an action to configure the DbContextOptionsBuilder for MainDbContext based on the provided parameters.
        /// </summary>
        /// <param name="connStr">The connection string for the database.</param>
        /// <param name="dbType">The database type (e.g., PgSql).</param>
        /// <param name="detailedLogging">If set to <c>true</c>, enables sensitive data logging.</param>
        /// <returns>An action that applies the necessary configuration settings to a DbContextOptionsBuilder instance.</returns>
        public static Action<DbContextOptionsBuilder> Configure(string connStr, DbType dbType, bool detailedLogging = false)
        {
            return (bldr) =>
            {
                // Configure the builder based on the specified database type.
                switch (dbType)
                {
                    case DbType.PgSql:
                        bldr.UseNpgsql(connStr,
                            opts => opts
                                // Set the command timeout to 10 minutes.
                                .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                                // Specify the migrations history table name.
                                .MigrationsHistoryTable("_migrations")
                                // Specify the migrations assembly based on the migration project prefix and database type.
                                .MigrationsAssembly($"{migrationProjectPrefix}{DbType.PgSql}")
                        );
                        break;
                        // Additional cases for other database types can be added here.
                }

                // Enable sensitive data logging if detailed logging is requested.
                if (detailedLogging)
                {
                    bldr.EnableSensitiveDataLogging();
                }

                // Enable lazy loading proxies for the context.
                bldr.UseLazyLoadingProxies(true);
                // Uncomment the following line to disable change tracking with identity resolution if needed.
                // bldr.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
            };
        }
    }
}
