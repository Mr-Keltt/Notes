using Notes.Context.Entities.Common;

namespace Notes.Context.Entities
{
    /// <summary>
    /// Represents a photo entity that stores the photo URL and its association with a note.
    /// </summary>
    public class PhotoEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the associated note data.
        /// </summary>
        public Guid NoteDataId { get; set; }

        /// <summary>
        /// Gets or sets the note data entity to which the photo belongs.
        /// </summary>
        public virtual NoteDataEntity NoteData { get; set; }

        /// <summary>
        /// Gets or sets the URL of the photo.
        /// </summary>
        public string Url { get; set; }
    }
}
