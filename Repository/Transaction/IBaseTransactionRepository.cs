using MicroFinance.Dto.Transactions;
using MicroFinance.Dtos;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper.TrasactionWrapper;

namespace MicroFinance.Repository.Transaction
{
    public interface IBaseTransactionRepository
    {
        Task<string> DepositAccountTransaction(DepositAccountTransactionWrapper depositAccountTransactionWrapper);
        Task<string> ShareAccountTransaction(ShareAccountTransactionWrapper shareAccountTransactionWrapper);
        Task ManualVoucherTransaction(List<ManualVoucherDto> manualVoucherDtos, string userBranchCode);
    }
}