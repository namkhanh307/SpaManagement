using AutoMapper;
using Repos.Entities;
using Repos.ViewModels.OrderVM;

namespace Services.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<PostOrderVM, Order>().ReverseMap();
            CreateMap<GetOrdersVM, Order>().ReverseMap();
            
        }
    }
}
