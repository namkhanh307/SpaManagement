using AutoMapper;
using Repos.Entities;
using Repos.ViewModels.PayRateVM;
using Repos.ViewModels.SalaryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
