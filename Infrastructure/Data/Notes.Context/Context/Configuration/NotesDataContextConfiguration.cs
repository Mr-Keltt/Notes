using Microsoft.EntityFrameworkCore;
using Notes.Context.Entities;

namespace Notes.Context
{
    /// <summary>
    /// Configures the entity mapping and relationships for the NoteDataEntity model.
    /// </summary>
    public static class NotesDataContextConfiguration
    {
        /// <summary>
        /// Configures the NoteDataEntity settings including table mapping, primary keys, indexes, and relationships.
        /// </summary>
        /// <param name="modelBuilder">The ModelBuilder instance used for configuring entity mappings.</param>
        public static void ConfigureNotesDatas(this ModelBuilder modelBuilder)
        {
            // Configure mapping and relationships for the NoteDataEntity.
            modelBuilder.Entity<NoteDataEntity>(entity =>
            {
                // Map the entity to the "notes_datas" table in the database.
                entity.ToTable("notes_datas");

                // Define the primary key for the entity.
                entity.HasKey(e => e.Uid);
                // Create a unique index on the primary key.
                entity.HasIndex(e => e.Uid).IsUnique();

                // Configure the Title property as required.
                entity.Property(e => e.Title)
                      .IsRequired();
                // Configure the Text property to use a column type of "text".
                entity.Property(e => e.Text)
                      .HasColumnType("text");
                // Configure the DateСhange property as required.
                entity.Property(e => e.DateСhange)
                      .IsRequired();
                // Configure the Marked property as required.
                entity.Property(e => e.Marked)
                      .IsRequired();
                // Configure the UserId property as required.
                entity.Property(e => e.UserId)
                      .IsRequired();

                // Set up a one-to-many relationship between User and NoteDataEntity.
                // Each note is linked to one user, and deleting the user will cascade delete the notes.
                entity.HasOne(e => e.User)
                      .WithMany(u => u.NotesDatas)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Set up a one-to-many relationship between NoteDataEntity and Photo.
                // Each note can have many photos, and deleting the note will cascade delete its photos.
                entity.HasMany(e => e.Photos)
                      .WithOne(p => p.NoteData)
                      .HasForeignKey(p => p.NoteDataId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
