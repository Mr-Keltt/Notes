using Microsoft.EntityFrameworkCore;
using Notes.Context.Entities;

namespace Notes.Context
{
    /// <summary>
    /// Configures the entity mapping and relationships for the UserEntity model.
    /// </summary>
    public static class UsersContextConfiguration
    {
        /// <summary>
        /// Configures the UserEntity settings including table mapping, primary keys, indexes, and relationships.
        /// </summary>
        /// <param name="modelBuilder">The ModelBuilder instance used for configuring entity mappings.</param>
        public static void ConfigureUsers(this ModelBuilder modelBuilder)
        {
            // Configure mapping and relationships for the UserEntity.
            modelBuilder.Entity<UserEntity>(entity =>
            {
                // Map the UserEntity to the "users" table in the database.
                entity.ToTable("users");

                // Define the primary key for the UserEntity.
                entity.HasKey(e => e.Uid);
                // Create a unique index on the primary key.
                entity.HasIndex(e => e.Uid).IsUnique();

                // Set up a one-to-many relationship between UserEntity and NoteDataEntity.
                // Each user can have many notes, and deleting a user will cascade delete their notes.
                entity.HasMany(e => e.NotesDatas)
                      .WithOne(n => n.User)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
