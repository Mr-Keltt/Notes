using Notes.Context.Entities.Common;

namespace Notes.Context.Entities;

public class PhotoEntity : BaseEntity
{
    public int NoteDataId { get; set; }
    public virtual NoteDataEntity NoteData { get; set; }

    public string Url { get; set; }
}
