using AutoMapper;
using Notes.Models;

namespace Notes.API.Models;

public class NoteDataResponse
{
    public Guid Uid { get; set; }
    public string Title { get; set; }
    public string? Text { get; set; }
    public DateTime DateChange { get; set; }
    public bool Marked { get; set; }
    public Guid UserId { get; set; }
    public ICollection<PhotoResponse> Photos { get; set; }
}


public class NoteDataResponseProfile : Profile
{
    public NoteDataResponseProfile()
    {
        CreateMap<NoteDataResponse, NoteDataModel>()
                .ForMember(dest => dest.DateСhange, opt => opt.MapFrom(src => src.DateChange))
                .ReverseMap();
    }
}