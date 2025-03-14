using AutoMapper;
using Notes.Models;

namespace Notes.API.Models
{
    /// <summary>
    /// Represents the API response model for note data, including associated photos.
    /// </summary>
    public class NoteDataResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the note.
        /// </summary>
        public Guid Uid { get; set; }

        /// <summary>
        /// Gets or sets the title of the note.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text content of the note.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the note was last changed.
        /// </summary>
        public DateTime DateChange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the note is marked.
        /// </summary>
        public bool Marked { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user who owns the note.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the collection of photo responses associated with the note.
        /// </summary>
        public ICollection<PhotoResponse> Photos { get; set; }
    }

    /// <summary>
    /// Configures the mapping between <see cref="NoteDataResponse"/> and <see cref="NoteDataModel"/> using AutoMapper.
    /// </summary>
    public class NoteDataResponseProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoteDataResponseProfile"/> class.
        /// This constructor sets up the mapping configuration.
        /// </summary>
        public NoteDataResponseProfile()
        {
            CreateMap<NoteDataResponse, NoteDataModel>()
                // Map the DateChange property from the response to the DateСhange property in the model.
                .ForMember(dest => dest.DateСhange, opt => opt.MapFrom(src => src.DateChange))
                .ReverseMap();
        }
    }
}
