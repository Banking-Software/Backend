using AutoMapper;
using MicroFinance.Dtos.Transactions;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper.TrasactionWrapper;

namespace MicroFinance.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {   
            CreateMap<MakeDepositTransactionDto, MakeDepositWrapper>();
            CreateMap<MakeDepositWrapper, BaseTransaction>();
        }
    }
}