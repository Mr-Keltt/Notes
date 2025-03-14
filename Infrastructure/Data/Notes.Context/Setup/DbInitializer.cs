using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Notes.Context
{
    /// <summary>
    /// Provides a method to initialize the database by applying any pending migrations.
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Executes the database migration process using the provided service provider.
        /// </summary>
        /// <param name="serviceProvider">
        /// The IServiceProvider instance from which to resolve required services for database initialization.
        /// </param>
        public static void Execute(IServiceProvider serviceProvider)
        {
            // Create a new service scope to ensure the correct lifetime for scoped services.
            using var scope = serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope();
            // Ensure that the scope is not null.
            ArgumentNullException.ThrowIfNull(scope);

            // Resolve the DbContext factory for MainDbContext from the scoped service provider.
            var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>();
            // Create a new instance of MainDbContext.
            using var context = dbContextFactory.CreateDbContext();
            // Apply any pending migrations to the database.
            context.Database.Migrate();
        }
    }
}
