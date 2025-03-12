using AutoMapper;
using Notes.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Models;

public class UserModel
{
    public Guid Uid { get; set; }
    public ICollection<NoteDataModel> NotesDatas { get; set; }
}


public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserModel>();
    }
}