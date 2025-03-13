using AutoMapper;
using Notes.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Models;

public class NoteDataModel
{
    public Guid Uid { get; set; }

    public string Title { get; set; }

    public string? Text { get; set; }

    public DateTime DateСhange { get; set; }

    public bool Marked { get; set; }

    public Guid UserId { get; set; }

    public ICollection<PhotoModel> Photos { get; set; }
}


public class NoteDataProfile : Profile
{
    public NoteDataProfile()
    {
        CreateMap<NoteDataEntity, NoteDataModel>()
            .ReverseMap();
    }
}