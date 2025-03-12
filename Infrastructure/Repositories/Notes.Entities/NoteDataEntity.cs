using Notes.Context.Entities.Common;

namespace Notes.Context.Entities;

public class NoteDataEntity : BaseEntity
{
    public string Name { get; set; }

    public string? Text { get; set; }

    public DateTime DateСhange { get; set; }

    public bool Marked { get; set; }

    public int UserId { get; set; }
    public virtual UserEntity User { get; set; }

    public virtual ICollection<PhotoEntity> Photos { get; set; }
}