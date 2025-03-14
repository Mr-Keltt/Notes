// Import necessary namespaces for object mapping and entity definitions.
using AutoMapper;
using Notes.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Models
{
    // This model represents the user data used in the application.
    // It contains a unique identifier for the user and a collection of notes associated with the user.
    public class UserModel
    {
        // A unique identifier for the user.
        public Guid Uid { get; set; }

        // A collection of notes associated with the user.
        public ICollection<NoteDataModel> NotesDatas { get; set; }
    }

    // This AutoMapper profile configures the mapping between UserEntity and UserModel.
    // It explicitly maps the NotesDatas property from the entity to the model,
    // and the mapping is bidirectional, allowing conversion in both directions.
    public class UserProfile : Profile
    {
        // Constructor where the mapping configuration is defined.
        public UserProfile()
        {
            // Create a bidirectional mapping between UserEntity and UserModel.
            // The NotesDatas property is explicitly mapped from the source entity to the destination model.
            CreateMap<UserEntity, UserModel>()
                .ForMember(dest => dest.NotesDatas, opt => opt.MapFrom(src => src.NotesDatas))
                .ReverseMap();
        }
    }
}
