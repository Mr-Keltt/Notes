using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Notes.Common.Settings;
using Notes.Context.Factories;

namespace Notes.Context
{
    /// <summary>
    /// Provides an extension method to register the application's database context and related services.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Registers the MainDbContext and its associated settings with the dependency injection container.
        /// </summary>
        /// <param name="services">The IServiceCollection to which the database context is added.</param>
        /// <param name="configuration">
        /// An optional IConfiguration instance from which to load the database settings.
        /// If not provided, default settings will be loaded.
        /// </param>
        /// <returns>The updated IServiceCollection to allow for method chaining.</returns>
        public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration = null)
        {
            // Load the database settings from the configuration using the "Database" section.
            var settings = Settings.Load<DbSettings>("Database", configuration);
            // Register the loaded DbSettings as a singleton service.
            services.AddSingleton(settings);

            // Configure the DbContextOptions for MainDbContext using the connection string, database type, and enabling detailed logging.
            var dbInitOptionsDelegate = DbContextOptionsFactory.Configure(settings.ConnectionString, settings.Type, true);

            // Register the DbContextFactory for MainDbContext with the configured options.
            services.AddDbContextFactory<MainDbContext>(dbInitOptionsDelegate);

            // Return the updated service collection.
            return services;
        }
    }
}
