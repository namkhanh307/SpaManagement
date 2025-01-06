using AutoMapper;
using Repos.Entities;
using Repos.ViewModels.PayRateVM;

namespace Services.Mappers
{
    public class PayRateProfile : Profile
    {
        public PayRateProfile()
        {
            CreateMap<PayRate, GetPayRatesVM>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty))
                .ReverseMap(); CreateMap<PostPayRateVM, PayRate>().ReverseMap();
        }
    }
}
