using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace Notes.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
    {
        public MainDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine("Getting started with the DesignTimeDbContextFactory...");

            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Attention: The arguments are not passed. We use 'pgsql' by default.");
            }
            else
            {
                Console.WriteLine($"Arguments: {string. Join(", ", args)}");
            }

            var provider = (args != null && args.Length > 0) ? args[0].ToLower() : "pgsql";
            Console.WriteLine($"Selected provider: {provider}");

            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine($"The path to the configuration file: {basePath}");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.context.json", optional: false, reloadOnChange: true)
                .Build();

            var connStr = configuration.GetConnectionString(provider);
            if (string.IsNullOrEmpty(connStr))
            {
                throw new Exception($"Error: Connection string for the provider '{provider}' not found in appsettings. context. json");
            }
            Console.WriteLine($"Connection string used: {connStr}");

            DbType dbType;
            if (provider == "pgsql")
                dbType = DbType.PgSql;
            else
                throw new Exception($"Mistake: Unsupported provider '{provider}'");

            var options = DbContextOptionsFactory.Create(connStr, dbType, detailedLogging: true);

            Console.WriteLine("DbContext has been created successfully!");
            return new MainDbContext(options);
        }
    }
}
