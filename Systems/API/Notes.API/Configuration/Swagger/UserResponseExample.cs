using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using Notes.API.Models;

namespace Notes.API.Configuration;

public class UserResponseExample : IExamplesProvider<UserResponse>
{
    public UserResponse GetExamples()
    {
        return new UserResponse
        {
            Uid = Guid.NewGuid(),
            NotesDatas = new List<NoteDataResponse>()
        };
    }
}