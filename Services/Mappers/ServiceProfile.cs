using AutoMapper;
using Repos.Entities;
using Repos.ViewModels.ProductVM;
using Repos.ViewModels.ServiceVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public  class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<Service, GetServicesVM>().ReverseMap();
            CreateMap<Service, PostServicesVM>().ReverseMap();
            CreateMap<Service, PutServicesVM>().ReverseMap();
        }
    }
}
