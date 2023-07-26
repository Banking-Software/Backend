using MicroFinance.Dtos;
using MicroFinance.Dtos.Transactions;

namespace MicroFinance.Services.Transactions
{
    public interface IDepositAccountTransactionService
    {
        Task<ResponseDto> MakeDepositTransactionService(MakeDepositTransactionDto makeDepositTransactionDto, TokenDto decodedToken);
    }
}