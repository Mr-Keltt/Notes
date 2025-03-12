using Microsoft.EntityFrameworkCore;
using Notes.Context.Entities;

namespace Notes.Context;

public class MainDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<NoteDataEntity> NotesDatas { get; set; }
    public DbSet<PhotoEntity> Photos { get; set; }
    
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureNotesDatas();
        modelBuilder.ConfigurePhotos();
        modelBuilder.ConfigureUsers();
    }
}