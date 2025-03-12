using AutoMapper;
using Notes.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Models;

public class PhotoModel
{
    public Guid Uid { get; set; }

    public Guid NoteDataId { get; set; }

    public string Url { get; set; }
}


public class PhotoProfile : Profile
{
    public PhotoProfile()
    {
        CreateMap<PhotoEntity, PhotoModel>();
    }
}