using AutoMapper;
using Repos.Entities;
using Repos.ViewModels.UserScheduleVM;

namespace Services.Mappers
{
    public class UserScheduleProfile : Profile
    {
        public UserScheduleProfile()
        {
            CreateMap<UserSchedule, GetUserScheduleVM>().ReverseMap();
            CreateMap<UserSchedule, PostUserScheduleVM>().ReverseMap();
        }
    }
}
