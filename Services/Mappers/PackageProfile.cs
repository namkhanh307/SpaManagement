using AutoMapper;
using Repos.Entities;
using Repos.ViewModels.PackageVM;

namespace Services.Mappers
{
    public class PackageProfile : Profile
    {
        public PackageProfile()
        {
            CreateMap<PostPackageVM, Package>().ReverseMap();
            CreateMap<GetPackagesVM, Package>().ReverseMap();
            CreateMap<PutPackageVM, Package>().ReverseMap();
        }
    }
}
