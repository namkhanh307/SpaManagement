using AutoMapper;
using Repos.Entities;
using Repos.ViewModels.SalaryVM;

namespace Services.Mappers
{
    public class SalaryProfile : Profile
    {
        public SalaryProfile()
        {
            CreateMap<GetSalariesVM, Salary>().ReverseMap().ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty));
            CreateMap<PostSalaryVM, Salary>().ReverseMap();
            CreateMap<PutSalaryVM, Salary>().ReverseMap();


        }
    }
}
