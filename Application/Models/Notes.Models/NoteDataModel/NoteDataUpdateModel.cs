using AutoMapper;
using Notes.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Models;

public class NoteDataUpdateModel
{
    public string Title { get; set; }

    public string? Text { get; set; }

    public bool Marked { get; set; }
}


public class NoteDataUpdateProfile : Profile
{
    public NoteDataUpdateProfile()
    {
        CreateMap<NoteDataUpdateModel, NoteDataEntity>()
            .ForMember(dest => dest.DateСhange, opt => opt.MapFrom(src => DateTime.Now));
    }
}
