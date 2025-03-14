using Notes.Context.Entities.Common;

namespace Notes.Context.Entities
{
    /// <summary>
    /// Represents a note data entity that stores note information including title, text, change date, and related user and photos.
    /// </summary>
    public class NoteDataEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the title of the note.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text content of the note.
        /// This field is optional.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the note was last changed.
        /// </summary>
        public DateTime DateСhange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the note is marked.
        /// </summary>
        public bool Marked { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user who owns the note.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the user entity associated with the note.
        /// </summary>
        public virtual UserEntity User { get; set; }

        /// <summary>
        /// Gets or sets the collection of photo entities associated with the note.
        /// </summary>
        public virtual ICollection<PhotoEntity> Photos { get; set; }
    }
}
