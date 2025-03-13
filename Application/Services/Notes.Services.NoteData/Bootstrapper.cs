using Microsoft.Extensions.DependencyInjection;

namespace Notes.Services.NoteData;

public static class Bootstrapper
{
    public static IServiceCollection AddNoteDataService(this IServiceCollection services)
    {
        services.AddScoped<INoteDataService, NoteDataService>();

        return services;
    }
}

