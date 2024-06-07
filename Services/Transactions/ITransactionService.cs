using MicroFinance.Dto.Transactions;
using MicroFinance.Dtos;

namespace MicroFinance.Services.Transactions
{
    public interface ITransactionService
    {
        Task<ResponseDto> ManualVoucherTransactionService(List<ManualVoucherDto> manualVouchers, TokenDto decodedToken);
    }
}