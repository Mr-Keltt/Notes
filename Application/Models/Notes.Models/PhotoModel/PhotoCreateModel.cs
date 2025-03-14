// Import necessary namespaces for object mapping and entity definitions.
using AutoMapper;
using Notes.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Models
{
    // This model represents the data required to create a new photo entry.
    // It contains the identifier of the associated note and the URL of the photo.
    public class PhotoCreateModel
    {
        // The unique identifier of the note to which the photo belongs.
        public Guid NoteDataId { get; set; }

        // The URL pointing to the location of the photo.
        public string Url { get; set; }
    }

    // This AutoMapper profile defines a bidirectional mapping between PhotoCreateModel and PhotoEntity.
    // This enables seamless conversion from the model to the entity and vice versa.
    public class PhotoCreateProfile : Profile
    {
        // Constructor where the mapping configuration is defined.
        public PhotoCreateProfile()
        {
            // Create a bidirectional mapping between PhotoCreateModel and PhotoEntity.
            CreateMap<PhotoCreateModel, PhotoEntity>()
                .ReverseMap();
        }
    }
}
