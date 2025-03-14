using Microsoft.EntityFrameworkCore;
using Notes.Context.Entities;

namespace Notes.Context
{
    /// <summary>
    /// Configures the entity mapping and relationships for the PhotoEntity model.
    /// </summary>
    public static class PhotosContextConfiguration
    {
        /// <summary>
        /// Configures the PhotoEntity settings including table mapping, primary keys, indexes, and relationships.
        /// </summary>
        /// <param name="modelBuilder">The ModelBuilder instance used for configuring entity mappings.</param>
        public static void ConfigurePhotos(this ModelBuilder modelBuilder)
        {
            // Configure mapping and relationships for the PhotoEntity.
            modelBuilder.Entity<PhotoEntity>(entity =>
            {
                // Map the PhotoEntity to the "photos" table in the database.
                entity.ToTable("photos");

                // Define the primary key for the PhotoEntity.
                entity.HasKey(e => e.Uid);
                // Create a unique index on the primary key.
                entity.HasIndex(e => e.Uid).IsUnique();

                // Configure the Url property as required.
                entity.Property(e => e.Url)
                      .IsRequired();
                // Configure the NoteDataId property as required.
                entity.Property(e => e.NoteDataId)
                      .IsRequired();

                // Set up a one-to-many relationship between NoteDataEntity and PhotoEntity.
                // Each photo is linked to one note, and deleting the note will cascade delete its photos.
                entity.HasOne(e => e.NoteData)
                      .WithMany(n => n.Photos)
                      .HasForeignKey(e => e.NoteDataId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
