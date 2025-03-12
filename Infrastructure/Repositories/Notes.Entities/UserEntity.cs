using Notes.Context.Entities.Common;

namespace Notes.Context.Entities;

public class UserEntity : BaseEntity
{
    public virtual ICollection<NoteDataEntity> NotesDatas { get; set; }
}