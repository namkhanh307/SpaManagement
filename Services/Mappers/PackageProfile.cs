using AutoMapper;
using Repos.Entities;
using Repos.ViewModels.PackageVM;

namespace Services.Mappers
{
    public class PackageProfile : Profile
    {
        public PackageProfile()
        {
            //Post
            CreateMap<Package, PostPackageVM>()
            .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.PackageServices.Select(ps => ps.Service))).ReverseMap();

            //Get
            CreateMap<Package, GetPackagesVM>()
            .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.PackageServices));

            //CreateMap<PackageService, GetPackageServiceVM>()
            //.ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type));

            CreateMap<PackageService, GetPackageServiceVM>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.Name))
            .ForMember(dest => dest.ServiceDuration, opt => opt.MapFrom(src => src.Service.Duration))
            .ForMember(dest => dest.ServiceStartPrice, opt => opt.MapFrom(src => src.Service.StartPrice))
            .ForMember(dest => dest.ServiceEndPrice, opt => opt.MapFrom(src => src.Service.EndPrice))
            .ForMember(dest => dest.ServiceDescription, opt => opt.MapFrom(src => src.Service.Description));

            //Put
            CreateMap<PutPackageVM, Package>().ReverseMap();
        }
    }
}
