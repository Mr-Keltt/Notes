using AutoMapper;
using Notes.Models;

namespace Notes.API.Models
{
    /// <summary>
    /// Represents the payload for creating a new note via an API request.
    /// </summary>
    public class NoteDataCreateRequest
    {
        /// <summary>
        /// Gets or sets the title of the note.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text content of the note.
        /// This property is optional.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the note is marked.
        /// </summary>
        public bool Marked { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user who owns the note.
        /// </summary>
        public Guid UserId { get; set; }
    }

    /// <summary>
    /// Configures the mapping between <see cref="NoteDataCreateRequest"/> and <see cref="NoteDataCreateModel"/> using AutoMapper.
    /// </summary>
    public class NoteDataCreateRequestProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoteDataCreateRequestProfile"/> class.
        /// This constructor sets up the mapping configuration.
        /// </summary>
        public NoteDataCreateRequestProfile()
        {
            CreateMap<NoteDataCreateRequest, NoteDataCreateModel>()
                .ReverseMap();
        }
    }
}
