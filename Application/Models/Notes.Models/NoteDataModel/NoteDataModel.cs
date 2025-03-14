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
    // This model represents the note data used in the application.
    // It contains properties such as a unique identifier, title, text, date of change,
    // a flag to mark important notes, the identifier of the user who owns the note,
    // and a collection of associated photos.
    public class NoteDataModel
    {
        // A unique identifier for the note.
        public Guid Uid { get; set; }

        // The title of the note.
        public string Title { get; set; }

        // Optional text content of the note.
        public string? Text { get; set; }

        // The date and time when the note was last changed.
        public DateTime DateСhange { get; set; }

        // A flag indicating if the note is marked (e.g., as important or starred).
        public bool Marked { get; set; }

        // The unique identifier of the user associated with the note.
        public Guid UserId { get; set; }

        // A collection of photos associated with the note.
        public ICollection<PhotoModel> Photos { get; set; }
    }

    // This AutoMapper profile defines how to map data between the NoteDataEntity
    // and the NoteDataModel. The mapping is bidirectional, allowing for conversion
    // from the entity to the model and vice versa.
    public class NoteDataProfile : Profile
    {
        // Constructor where the mapping configuration is defined.
        public NoteDataProfile()
        {
            // Create a bidirectional mapping between NoteDataEntity and NoteDataModel.
            CreateMap<NoteDataEntity, NoteDataModel>()
                .ReverseMap();
        }
    }
}
