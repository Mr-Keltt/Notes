using Microsoft.EntityFrameworkCore;
using Notes.Context.Entities;

namespace Notes.Context;

public static class PhotosContextConfiguration
{
    public static void ConfigurePhotos(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PhotoEntity>(entity =>
        {
            entity.ToTable("photos");

            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Uid).IsUnique();

            entity.Property(e => e.Url)
                      .IsRequired();
            entity.Property(e => e.NotesDataId)
                  .IsRequired();

            entity.HasOne(e => e.NotesData)
                      .WithMany(n => n.Photos)
                      .HasForeignKey(e => e.NotesDataId)
                      .OnDelete(DeleteBehavior.Cascade);
        });
    }
}