using Microsoft.EntityFrameworkCore;

namespace Notes.Context.Factories
{
    /// <summary>
    /// Provides a factory for creating instances of the MainDbContext using predefined options.
    /// </summary>
    public class DbContextFactory
    {
        // The options used to configure instances of MainDbContext.
        private readonly DbContextOptions<MainDbContext> options;

        /// <summary>
        /// Initializes a new instance of the DbContextFactory class with the specified DbContext options.
        /// </summary>
        /// <param name="options">The options used to configure the MainDbContext.</param>
        public DbContextFactory(DbContextOptions<MainDbContext> options)
        {
            this.options = options;
        }

        /// <summary>
        /// Creates and returns a new instance of MainDbContext configured with the provided options.
        /// </summary>
        /// <returns>A new instance of MainDbContext.</returns>
        public MainDbContext Create()
        {
            return new MainDbContext(options);
        }
    }
}
