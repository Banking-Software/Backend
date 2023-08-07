using MicroFinance.Dtos;
using MicroFinance.Dtos.Transactions.ShareTransaction;

namespace MicroFinance.Services.Transactions
{
    public interface IShareAccountTransactionService
    {
        Task<string> MakeShareTransaction(MakeShareTransactionDto makeShareTransactionDto, TokenDto decodedToken);
    }
}