using Notes.Context.Entities.Common;

namespace Notes.Context.Entities
{
    /// <summary>
    /// Represents a user entity that stores user-specific information and the associated notes.
    /// </summary>
    public class UserEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the collection of notes associated with the user.
        /// </summary>
        public virtual ICollection<NoteDataEntity> NotesDatas { get; set; }
    }
}
