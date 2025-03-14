using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Notes.Context.Seeder;

public static class Bootstrapper
{
    public static IServiceCollection AddDbSeeder(this IServiceCollection services, IConfiguration configuration = null)
    {


        return services;
    }
}

