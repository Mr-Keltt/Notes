using AutoMapper;
using Notes.Models;

namespace Notes.API.Models;

public class PhotoResponse
{
    public Guid Uid { get; set; }
    public Guid NoteDataId { get; set; }
    public string Url { get; set; }
}

public class PhotoResponseProfile : Profile
{
    public PhotoResponseProfile()
    {
        CreateMap<PhotoResponse, PhotoModel>()
            .ReverseMap();
    }
}