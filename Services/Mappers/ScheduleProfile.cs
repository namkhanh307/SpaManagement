using AutoMapper;
using Repos.Entities;
using Repos.ViewModels.ScheduleVM;

namespace Services.Mappers
{
    public class ScheduleProfile : Profile
    {
        public ScheduleProfile()
        {
            CreateMap<Schedule, GetScheduleVM>().ReverseMap();
            CreateMap<Schedule, PostScheduleVM>().ReverseMap();
        }
    }
}
