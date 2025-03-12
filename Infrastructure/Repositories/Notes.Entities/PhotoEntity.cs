using Notes.Context.Entities.Common;

namespace Notes.Context.Entities;

public class PhotoEntity : BaseEntity
{
    public int NotesDataId { get; set; }
    public virtual NoteDataEntity NotesData { get; set; }

    public string Url { get; set; }
}
