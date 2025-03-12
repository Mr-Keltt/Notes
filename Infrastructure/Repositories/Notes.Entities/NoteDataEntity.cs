using Notes.Context.Entities.Common;

namespace Notes.Context.Entities;

public class NoteDataEntity : BaseEntity
{
    public string Title { get; set; }

    public string? Text { get; set; }

    public DateTime DateСhange { get; set; }

    public bool Marked { get; set; }

    public Guid UserId { get; set; }
    public virtual UserEntity User { get; set; }

    public virtual ICollection<PhotoEntity> Photos { get; set; }
}