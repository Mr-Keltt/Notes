using AutoMapper;
using Notes.Models;

namespace Notes.API.Models;

public class PhotoCreateRequest
{
    public Guid NoteDataId { get; set; }
    public string Url { get; set; }
}

public class PhotoCreateRequestProfile : Profile
{
    public PhotoCreateRequestProfile()
    {
        CreateMap<PhotoCreateRequest, PhotoCreateModel>()
            .ReverseMap();
    }
}