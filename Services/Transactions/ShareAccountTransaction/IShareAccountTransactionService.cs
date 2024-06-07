using MicroFinance.Dtos;
using MicroFinance.Dtos.Transactions.ShareTransaction;

namespace MicroFinance.Services.Transactions
{
    public interface IShareAccountTransactionService
    {
        Task<VoucherDto> MakeShareTransaction(MakeShareTransactionDto makeShareTransactionDto, TokenDto decodedToken);
    }
}