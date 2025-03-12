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
            entity.Property(e => e.NoteDataId)
                  .IsRequired();

            entity.HasOne(e => e.NoteData)
                      .WithMany(n => n.Photos)
                      .HasForeignKey(e => e.NoteDataId)
                      .OnDelete(DeleteBehavior.Cascade);
        });
    }
}