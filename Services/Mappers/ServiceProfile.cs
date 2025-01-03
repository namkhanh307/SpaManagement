using AutoMapper;
using Repos.Entities;
using Repos.ViewModels.ServiceVM;

namespace Services.Mappers
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<Service, GetServicesVM>().ReverseMap();
            CreateMap<Service, PostServiceVM>().ReverseMap();
            CreateMap<Service, PutServiceVM>().ReverseMap();
        }
    }
}
