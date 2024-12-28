using AutoMapper;
using Repos.Entities;
using Repos.ViewModels.ProductVM;

namespace Services.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, GetProductsVM>().ReverseMap();
            CreateMap<Product, PostProductVM>().ReverseMap();
            CreateMap<Product , PutProductVM>().ReverseMap(); 
        }
    }
}
