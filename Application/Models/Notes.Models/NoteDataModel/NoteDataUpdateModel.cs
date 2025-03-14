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
    // This model defines the properties required to update an existing note.
    // It includes the note's title, optional text content, and a flag to indicate whether the note is marked.
    public class NoteDataUpdateModel
    {
        // The title of the note.
        public string Title { get; set; }

        // Optional text content of the note. Can be null if not provided.
        public string? Text { get; set; }

        // A flag indicating if the note is marked (e.g., as important or highlighted).
        public bool Marked { get; set; }
    }

    // This AutoMapper profile configures the mapping between NoteDataUpdateModel and NoteDataEntity.
    // It ensures that when an update occurs, the 'DateСhange' property of the entity is set to the current UTC time.
    public class NoteDataUpdateProfile : Profile
    {
        // The constructor sets up the mapping configuration.
        public NoteDataUpdateProfile()
        {
            // Create a mapping from NoteDataUpdateModel to NoteDataEntity.
            // The 'DateСhange' property is automatically updated to the current UTC time during mapping.
            CreateMap<NoteDataUpdateModel, NoteDataEntity>()
                .ForMember(dest => dest.DateСhange, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
