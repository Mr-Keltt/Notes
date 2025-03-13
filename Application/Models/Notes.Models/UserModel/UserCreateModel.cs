using AutoMapper;
using Notes.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Models;

public class UserCreateModel
{
    public ICollection<NoteDataModel> NotesDatas { get; set; }
}


public class UserCreateProfile : Profile
{
    public UserCreateProfile()
    {
        CreateMap<UserCreateModel, UserEntity>()
            .ForMember(dest => dest.NotesDatas, opt => opt.MapFrom(src => src.NotesDatas));
    }
}