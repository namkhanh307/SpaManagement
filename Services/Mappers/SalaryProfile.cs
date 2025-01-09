using AutoMapper;
using Repos.Entities;
using Repos.ViewModels.SalaryVM;

namespace Services.Mappers
{
    public class SalaryProfile : Profile
    {
        public SalaryProfile()
        {
            CreateMap<GetSalariesVM, Salary>().ReverseMap();
            CreateMap<PostSalaryVM, Salary>().ReverseMap();

        }
    }
}
