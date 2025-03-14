using Microsoft.Extensions.DependencyInjection;

namespace Notes.Services.User
{
    /// <summary>
    /// Provides an extension method to register the user service with the dependency injection container.
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Registers the IUserService interface and its implementation UserService as a scoped service in the service collection.
        /// </summary>
        /// <param name="services">The IServiceCollection to which the user service is added.</param>
        /// <returns>The updated IServiceCollection to allow for method chaining.</returns>
        public static IServiceCollection AddUserService(this IServiceCollection services)
        {
            // Register UserService as the implementation of IUserService.
            services.AddScoped<IUserService, UserService>();

            // Return the updated service collection.
            return services;
        }
    }
}
