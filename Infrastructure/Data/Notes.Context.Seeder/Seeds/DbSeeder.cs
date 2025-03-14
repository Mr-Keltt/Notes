using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Notes.Context;
using Notes.Models;
using Notes.Services.NoteData;
using Notes.Services.Photo;
using Notes.Services.User;
using System.Reflection;

namespace ZooExpoOrg.Context;

public static class DbSeeder
{
    private static IServiceScope ServiceScope(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetService<IServiceScopeFactory>()!.CreateScope();
    }

    private static MainDbContext DbContext(IServiceProvider serviceProvider)
    {
        return ServiceScope(serviceProvider)
            .ServiceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>().CreateDbContext();
    }

    public static void Execute(IServiceProvider serviceProvider)
    {
        Task.Run(async () =>
        {
            await AddDemoData(serviceProvider);
        })
            .GetAwaiter()
            .GetResult();
    }

    private static async Task AddDemoData(IServiceProvider serviceProvider)
    {
        using var scope = ServiceScope(serviceProvider);
        if (scope == null)
            return;

        var settings = scope.ServiceProvider.GetService<DbSettings>();
        if (!(settings.Init?.AddDemoData ?? false))
            return;

        await using var context = DbContext(serviceProvider);

        if (await context.Users.AnyAsync())
        {
            return;
        }

        var userService = scope.ServiceProvider.GetService<IUserService>();
        var noteDataService = scope.ServiceProvider.GetService<INoteDataService>();
        var photoService = scope.ServiceProvider.GetService<IPhotoService>();

        var user = await userService.CreateAsync();

        var note = await noteDataService.CreateNoteAsync(new NoteDataCreateModel()
        {
            Title = "Пример заметки",
            Text = "\r\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque purus purus, mattis ac interdum et, tempor a libero. Mauris mollis sagittis quam, nec scelerisque eros mollis eget. Pellentesque accumsan congue commodo. Praesent eget auctor ipsum. Sed quis convallis turpis. In hendrerit consequat porttitor. Nunc eu mauris purus. Nullam iaculis vulputate ex, nec iaculis arcu consectetur ut. Praesent efficitur, lorem nec dapibus pulvinar, dolor nisi finibus diam, vel vestibulum magna mauris semper est. Pellentesque convallis semper sollicitudin. Nulla tincidunt purus arcu, cursus venenatis arcu efficitur vitae. Morbi lobortis faucibus libero vitae vestibulum. Aliquam erat volutpat.",
            Marked = false,
            UserId = user.Uid
        });
    }
}
