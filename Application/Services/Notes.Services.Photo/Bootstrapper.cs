using Microsoft.Extensions.DependencyInjection;

namespace Notes.Services.Photo;

public static class Bootstrapper
{
    public static IServiceCollection AddUserService(this IServiceCollection services)
    {
        services.AddScoped<IPhotoService, PhotoService>();

        return services;
    }
}
