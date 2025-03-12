using AutoMapper;
using Notes.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Models;

public class PhotoUpdateModel
{
    public string Url { get; set; }
}


public class PhotoUpdateProfile : Profile
{
    public PhotoUpdateProfile()
    {
        CreateMap<PhotoUpdateModel, PhotoEntity>();
    }
}