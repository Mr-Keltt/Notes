using AutoMapper;
using Notes.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Models;

public class PhotoCreateModel
{
    public Guid NoteDataId { get; set; }
    public string Url { get; set; }
}


public class PhotoCreateProfile : Profile
{
    public PhotoCreateProfile()
    {
        CreateMap<PhotoCreateModel, PhotoEntity>()
            .ReverseMap();
    }
}