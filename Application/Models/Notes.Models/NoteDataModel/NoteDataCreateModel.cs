// Import necessary namespaces for entity definitions and object mapping.
using Notes.Context.Entities;
using AutoMapper;

namespace Notes.Models
{
    // This model represents the data required to create a new note.
    // It contains properties for the note's title, optional text content,
    // a flag indicating whether the note is marked, and the identifier of the user creating the note.
    public class NoteDataCreateModel
    {
        // The title of the note.
        public string Title { get; set; }

        // The optional text content of the note. This can be null if no text is provided.
        public string? Text { get; set; }

        // A flag indicating whether the note is marked (e.g., important or highlighted).
        public bool Marked { get; set; }

        // The unique identifier of the user who created the note.
        public Guid UserId { get; set; }
    }

    // This AutoMapper profile defines how to map data from the NoteDataCreateModel
    // to the NoteDataEntity. It ensures that the properties of the model are correctly
    // transformed into the corresponding entity properties.
    public class NoteDataCreateProfile : Profile
    {
        // The constructor sets up the mapping configuration.
        public NoteDataCreateProfile()
        {
            // Map properties from NoteDataCreateModel to NoteDataEntity.
            // The DateСhange property in the destination entity is set to the current UTC time.
            CreateMap<NoteDataCreateModel, NoteDataEntity>()
                .ForMember(dest => dest.DateСhange, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
