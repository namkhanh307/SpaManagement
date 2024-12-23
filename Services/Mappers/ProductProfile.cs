using AutoMapper;
using Repos.Entities;
using Repos.ViewModels.ProductVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, GetProductsVM>().ReverseMap();
            CreateMap<Product, PostProductVM>().ReverseMap();
        }
    }
}
