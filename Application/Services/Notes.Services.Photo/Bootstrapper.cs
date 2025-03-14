using Microsoft.Extensions.DependencyInjection;

namespace Notes.Services.Photo
{
    /// <summary>
    /// Provides an extension method to register the photo service with the dependency injection container.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Registers the IPhotoService interface and its implementation PhotoService as a scoped service.
        /// </summary>
        /// <param name="services">The IServiceCollection to which the photo service is added.</param>
        /// <returns>The updated IServiceCollection for method chaining.</returns>
        public static IServiceCollection AddPhotoService(this IServiceCollection services)
        {
            // Register PhotoService as the implementation for IPhotoService.
            services.AddScoped<IPhotoService, PhotoService>();

            // Return the updated service collection.
            return services;
        }
    }
}
