using AutoMapper;
using Notes.Models;

namespace Notes.API.Models;

public class UserResponse
{
    public Guid Uid { get; set; }
    public ICollection<NoteDataResponse> NotesDatas { get; set; }
}

public class UserResponseProfile : Profile
{
    public UserResponseProfile()
    {
        CreateMap<UserResponse, UserModel>()
            .ReverseMap();
    }
}