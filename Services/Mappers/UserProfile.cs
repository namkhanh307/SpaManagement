using AutoMapper;
using Repos.Entities;
using Repos.ViewModels.UserVM;

namespace Services.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<GetUsersVM, User>().ReverseMap();
            CreateMap<PostUserVM, User>().ReverseMap();
        }
    }
}
