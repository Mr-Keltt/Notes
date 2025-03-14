using AutoMapper;
using Notes.Models;

namespace Notes.API.Models
{
    /// <summary>
    /// Represents the API response model for a user, including the user's unique identifier and associated notes.
    /// </summary>
    public class UserResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public Guid Uid { get; set; }

        /// <summary>
        /// Gets or sets the collection of note responses associated with the user.
        /// </summary>
        public ICollection<NoteDataResponse> NotesDatas { get; set; }
    }

    /// <summary>
    /// Configures the mapping between <see cref="UserResponse"/> and <see cref="UserModel"/> using AutoMapper.
    /// </summary>
    public class UserResponseProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserResponseProfile"/> class.
        /// This constructor sets up the mapping configuration.
        /// </summary>
        public UserResponseProfile()
        {
            CreateMap<UserResponse, UserModel>()
                .ReverseMap();
        }
    }
}
