using AutoMapper;
using Repos.Entities;
using Repos.ViewModels.PackageVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public  class PackageProfile : Profile
    {
        public PackageProfile()
        {
            CreateMap<PostPackageVM, Package>().ReverseMap();
            CreateMap<GetPackagesVM, Package>().ReverseMap();
            CreateMap<PutPackageVM, Package>().ReverseMap();
        }
    }
}
