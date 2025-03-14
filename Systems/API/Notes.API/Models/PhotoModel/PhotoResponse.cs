using AutoMapper;
using Notes.Models;

namespace Notes.API.Models
{
    /// <summary>
    /// Represents the API response model for a photo, including its unique identifier, the associated note's identifier, and the photo URL.
    /// </summary>
    public class PhotoResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the photo.
        /// </summary>
        public Guid Uid { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the note to which this photo belongs.
        /// </summary>
        public Guid NoteDataId { get; set; }

        /// <summary>
        /// Gets or sets the URL of the photo.
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// Configures the mapping between <see cref="PhotoResponse"/> and <see cref="PhotoModel"/> using AutoMapper.
    /// </summary>
    public class PhotoResponseProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoResponseProfile"/> class.
        /// This constructor sets up the mapping configuration.
        /// </summary>
        public PhotoResponseProfile()
        {
            CreateMap<PhotoResponse, PhotoModel>()
                .ReverseMap();
        }
    }
}
