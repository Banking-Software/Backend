using MicroFinance.Dtos;
using MicroFinance.Dtos.Transactions;

namespace MicroFinance.Services.Transactions
{
    public interface IDepositAccountTransactionService
    {
        Task<VoucherDto> MakeDepositTransactionService(MakeDepositTransactionDto makeDepositTransactionDto, TokenDto decodedToken);
        Task<VoucherDto> MakeWithDrawalTransactionService(MakeWithDrawalTransactionDto makeWithDrawalTransactionDto, TokenDto decodedToken);

    }
}