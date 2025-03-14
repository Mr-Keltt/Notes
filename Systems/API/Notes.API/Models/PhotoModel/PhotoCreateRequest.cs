using AutoMapper;
using Notes.Models;

namespace Notes.API.Models
{
    /// <summary>
    /// Represents the API request model used to create a new photo.
    /// </summary>
    public class PhotoCreateRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the note to which the photo belongs.
        /// </summary>
        public Guid NoteDataId { get; set; }

        /// <summary>
        /// Gets or sets the URL of the photo.
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// Configures the mapping between <see cref="PhotoCreateRequest"/> and <see cref="PhotoCreateModel"/> using AutoMapper.
    /// </summary>
    public class PhotoCreateRequestProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoCreateRequestProfile"/> class.
        /// This constructor sets up the mapping configuration.
        /// </summary>
        public PhotoCreateRequestProfile()
        {
            CreateMap<PhotoCreateRequest, PhotoCreateModel>()
                .ReverseMap();
        }
    }
}
