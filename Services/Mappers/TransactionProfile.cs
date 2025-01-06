using AutoMapper;
using Repos.ViewModels.TransactionVM;
using System.Transactions;

namespace Services.Mappers
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, GetTransactionVM>().ReverseMap();
            CreateMap<Transaction, PostTransactionVM>().ReverseMap();
        }
    }
}
