using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace Notes.Context.Factories
{
    /// <summary>
    /// Provides a factory for creating instances of MainDbContext during design-time.
    /// This is useful for Entity Framework Core tools, such as migrations.
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
    {
        /// <summary>
        /// Creates a new instance of MainDbContext using the specified design-time arguments.
        /// </summary>
        /// <param name="args">Command-line arguments specifying the database provider.</param>
        /// <returns>A configured instance of MainDbContext.</returns>
        public MainDbContext CreateDbContext(string[] args)
        {
            // Log the start of the factory process.
            Console.WriteLine("Getting started with the DesignTimeDbContextFactory...");

            // Check if any arguments were provided; if not, default to "pgsql".
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Attention: The arguments are not passed. We use 'pgsql' by default.");
            }
            else
            {
                Console.WriteLine($"Arguments: {string.Join(", ", args)}");
            }

            // Determine the provider based on the first argument, defaulting to "pgsql" if not provided.
            var provider = args != null && args.Length > 0 ? args[0].ToLower() : "pgsql";
            Console.WriteLine($"Selected provider: {provider}");

            // Get the directory path where the executing assembly is located.
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine($"The path to the configuration file: {basePath}");

            // Build the configuration by loading the appsettings.context.json file from the base path.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.context.json", optional: false, reloadOnChange: true)
                .Build();

            // Retrieve the connection string for the selected provider.
            var connStr = configuration.GetConnectionString(provider);
            if (string.IsNullOrEmpty(connStr))
            {
                throw new Exception($"Error: Connection string for the provider '{provider}' not found in appsettings.context.json");
            }
            Console.WriteLine($"Connection string used: {connStr}");

            // Determine the DbType based on the selected provider.
            DbType dbType;
            if (provider == "pgsql")
                dbType = DbType.PgSql;
            else
                throw new Exception($"Mistake: Unsupported provider '{provider}'");

            // Create the DbContextOptions for MainDbContext using the connection string and database type.
            var options = DbContextOptionsFactory.Create(connStr, dbType, detailedLogging: true);

            // Log the successful creation of the DbContext.
            Console.WriteLine("DbContext has been created successfully!");
            return new MainDbContext(options);
        }
    }
}
