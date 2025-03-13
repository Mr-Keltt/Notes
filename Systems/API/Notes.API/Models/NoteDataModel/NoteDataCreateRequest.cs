using AutoMapper;
using Notes.Models;

namespace Notes.API.Models;

public class NoteDataCreateRequest
{
    public string Title { get; set; }
    public string? Text { get; set; }
    public bool Marked { get; set; }
    public Guid UserId { get; set; }
}

public class NoteDataCreateRequestProfile : Profile
{
    public NoteDataCreateRequestProfile()
    {
        CreateMap<NoteDataCreateRequest, NoteDataCreateModel>()
            .ReverseMap();
    }
}