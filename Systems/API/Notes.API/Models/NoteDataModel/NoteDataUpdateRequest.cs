using AutoMapper;
using Notes.Models;

namespace Notes.API.Models;

public class NoteDataUpdateRequest
{
    public string Title { get; set; }
    public string? Text { get; set; }
    public bool Marked { get; set; }
}

public class NoteDataUpdateRequestProfile : Profile
{
    public NoteDataUpdateRequestProfile()
    {
        CreateMap<NoteDataUpdateRequest, NoteDataUpdateModel>()
            .ReverseMap();
    }
}
