using Notes.Context.Entities;
using AutoMapper;

namespace Notes.Models;

public class NoteDataCreateModel
{
    public string Title { get; set; }

    public string? Text { get; set; }

    public bool Marked { get; set; }

    public Guid UserId { get; set; }

    public ICollection<PhotoCreateModel> Photos { get; set; }
}


public class NoteDataCreateProfile : Profile
{
    public NoteDataCreateProfile()
    {
        CreateMap<NoteDataCreateModel, NoteDataEntity>()
            .ForMember(dest => dest.DateСhange, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos));
    }
}