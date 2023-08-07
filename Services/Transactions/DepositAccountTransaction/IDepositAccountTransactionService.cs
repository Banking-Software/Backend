using MicroFinance.Dtos;
using MicroFinance.Dtos.Transactions;

namespace MicroFinance.Services.Transactions
{
    public interface IDepositAccountTransactionService
    {
        Task<string> MakeDepositTransactionService(MakeDepositTransactionDto makeDepositTransactionDto, TokenDto decodedToken);
        Task<string> MakeWithDrawalTransactionService(MakeWithDrawalTransactionDto makeWithDrawalTransactionDto, TokenDto decodedToken);

    }
}