using Microsoft.Extensions.DependencyInjection;
using Notes.Models;

namespace Notes.Services.NoteData;

public static class Bootstrapper
{
    public static IServiceCollection AddApplicationModels(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserProfile), typeof(NoteDataProfile), typeof(PhotoProfile));

        return services;
    }
}

