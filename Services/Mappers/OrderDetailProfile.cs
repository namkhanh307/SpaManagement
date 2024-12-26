using AutoMapper;
using Repos.Entities;
using Repos.ViewModels.OrderDetailVM;

namespace Services.Mappers
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<OrderDetail, GetOrderDetailVM>().ReverseMap();
            CreateMap<OrderDetail, PostOrderDetailVM>().ReverseMap();
        }
    }
}
