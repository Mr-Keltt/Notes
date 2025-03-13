using AutoMapper;
using Notes.Models;

namespace Notes.API.Models;

public class UserCreateRequest
{
    public ICollection<NoteDataResponse> NotesDatas { get; set; }
}

public class UserCreateRequestProfile : Profile
{
    public UserCreateRequestProfile()
    {
        CreateMap<UserCreateRequest, UserCreateModel>()
            .ReverseMap();
    }
}