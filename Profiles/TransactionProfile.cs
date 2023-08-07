using AutoMapper;
using MicroFinance.Dtos.Transactions;
using MicroFinance.Dtos.Transactions.ShareTransaction;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper.TrasactionWrapper;

namespace MicroFinance.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {   
            CreateMap<MakeDepositTransactionDto, DepositAccountTransactionWrapper>();
            CreateMap<MakeWithDrawalTransactionDto, DepositAccountTransactionWrapper>();
            CreateMap<DepositAccountTransactionWrapper, BaseTransaction>();
            CreateMap<MakeShareTransactionDto, ShareAccountTransactionWrapper>();
            CreateMap<ShareAccountTransactionWrapper, BaseTransaction>();
        }
    }
}