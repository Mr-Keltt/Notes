using Microsoft.Extensions.DependencyInjection;

namespace Notes.Services.NoteData
{
    /// <summary>
    /// Provides an extension method to register the NoteDataService with the application's dependency injection container.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Registers the INoteDataService interface and its implementation NoteDataService with the service collection.
        /// </summary>
        /// <param name="services">The service collection to add the service registration to.</param>
        /// <returns>The updated service collection with the NoteDataService registration.</returns>
        public static IServiceCollection AddNoteDataService(this IServiceCollection services)
        {
            // Register NoteDataService as a scoped service for dependency injection.
            services.AddScoped<INoteDataService, NoteDataService>();

            // Return the modified service collection to support method chaining.
            return services;
        }
    }
}
