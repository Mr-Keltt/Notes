using Microsoft.EntityFrameworkCore;
using Notes.Context.Entities;

namespace Notes.Context
{
    /// <summary>
    /// Represents the main database context for the Notes application, managing entity sets and their configurations.
    /// </summary>
    public class MainDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the collection of UserEntity objects.
        /// </summary>
        public virtual DbSet<UserEntity> Users { get; set; }
        /// <summary>
        /// Gets or sets the collection of NoteDataEntity objects.
        /// </summary>
        public virtual DbSet<NoteDataEntity> NotesDatas { get; set; }
        /// <summary>
        /// Gets or sets the collection of PhotoEntity objects.
        /// </summary>
        public virtual DbSet<PhotoEntity> Photos { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainDbContext class using the specified options.
        /// </summary>
        /// <param name="options">The options for configuring the DbContext.</param>
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

        /// <summary>
        /// Configures the entity mappings and relationships for the application.
        /// </summary>
        /// <param name="modelBuilder">The ModelBuilder used to configure the entity mappings.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configuration for NoteDataEntity.
            modelBuilder.ConfigureNotesDatas();
            // Apply configuration for PhotoEntity.
            modelBuilder.ConfigurePhotos();
            // Apply configuration for UserEntity.
            modelBuilder.ConfigureUsers();
        }
    }
}
