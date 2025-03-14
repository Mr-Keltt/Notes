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
    // This model represents the photo data used within the application.
    // It contains properties such as a unique identifier, the associated note identifier,
    // and the URL where the photo is stored.
    public class PhotoModel
    {
        // A unique identifier for the photo.
        public Guid Uid { get; set; }

        // The unique identifier of the note associated with this photo.
        public Guid NoteDataId { get; set; }

        // The URL pointing to the photo's location.
        public string Url { get; set; }
    }

    // This AutoMapper profile configures the mapping between PhotoEntity and PhotoModel.
    // The mapping is bidirectional, allowing conversion from the entity to the model and vice versa.
    public class PhotoProfile : Profile
    {
        // Constructor where the mapping configuration is defined.
        public PhotoProfile()
        {
            // Create a bidirectional mapping between PhotoEntity and PhotoModel.
            CreateMap<PhotoEntity, PhotoModel>()
                .ReverseMap();
        }
    }
}
