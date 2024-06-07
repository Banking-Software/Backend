using MicroFinance.Dto.Transactions;

namespace MicroFinance.Repository.Transaction;

public interface IManualVoucherTransactionRepository
{
    Task ManualVoucherTransaction(List<ManualVoucherDto> manualVouchers, string userBranchCode);
}