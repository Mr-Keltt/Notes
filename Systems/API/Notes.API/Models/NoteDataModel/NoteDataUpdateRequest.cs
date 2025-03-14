using AutoMapper;
using Notes.Models;

namespace Notes.API.Models
{
    /// <summary>
    /// Represents the API request model used to update note data.
    /// </summary>
    public class NoteDataUpdateRequest
    {
        /// <summary>
        /// Gets or sets the updated title of the note.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the updated text content of the note.
        /// This property is optional.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the note is marked.
        /// </summary>
        public bool Marked { get; set; }
    }

    /// <summary>
    /// Configures the mapping between <see cref="NoteDataUpdateRequest"/> and <see cref="NoteDataUpdateModel"/> using AutoMapper.
    /// </summary>
    public class NoteDataUpdateRequestProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoteDataUpdateRequestProfile"/> class.
        /// This constructor sets up the mapping configuration.
        /// </summary>
        public NoteDataUpdateRequestProfile()
        {
            CreateMap<NoteDataUpdateRequest, NoteDataUpdateModel>()
                .ReverseMap();
        }
    }
}
