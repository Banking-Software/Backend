using MicroFinance.Dto.Transactions;
using MicroFinance.Models.Wrapper.TrasactionWrapper;

namespace MicroFinance.Repository.Transaction
{
    public class BaseTransactionRepository : IBaseTransactionRepository
    {
        private readonly IDepositAccountTransactionRepository _depositAccountTransaction;
        private readonly IShareAccountTransactionRepository _shareAccountTransaction;
        private readonly IManualVoucherTransactionRepository _manualVoucherTransaction;
        private readonly ILogger<BaseTransactionRepository> _logger;
        SemaphoreSlim transactionLock = new SemaphoreSlim(1, 1);
        public BaseTransactionRepository
        (
            IDepositAccountTransactionRepository depositAccountTransaction, 
            IShareAccountTransactionRepository shareAccountTransaction,
            IManualVoucherTransactionRepository manualVoucherTransaction,
            ILogger<BaseTransactionRepository> logger
        )
        {
            _depositAccountTransaction=depositAccountTransaction;
            _shareAccountTransaction=shareAccountTransaction;
            _manualVoucherTransaction=manualVoucherTransaction;
            _logger=logger;
        }
        public async Task<string> DepositAccountTransaction(DepositAccountTransactionWrapper depositAccountTransactionWrapper)
        {
            try
            {
                if(await transactionLock.WaitAsync(TimeSpan.FromMinutes(1)))
                    return await _depositAccountTransaction.HandleDepositAccountTransaction(depositAccountTransactionWrapper);
                else
                    throw new TimeoutException("Transaction time out. Please try again...");
            }
            finally
            {
                transactionLock.Release();
            }
        }

        public async Task ManualVoucherTransaction(List<ManualVoucherDto> manualVoucherDtos, string userBranchCode)
        {
            try
            {
                if(await transactionLock.WaitAsync(TimeSpan.FromMinutes(1)))
                    await _manualVoucherTransaction.ManualVoucherTransaction(manualVoucherDtos, userBranchCode);
                else
                    throw new TimeoutException("Transaction time out. Please try again...");
            }
            finally
            {
                transactionLock.Release();
            }
        }

        public async Task<string> ShareAccountTransaction(ShareAccountTransactionWrapper shareAccountTransactionWrapper)
        {
            try
            {
                if(await transactionLock.WaitAsync(TimeSpan.FromMinutes(1)))
                    return await _shareAccountTransaction.HandleShareTransaction(shareAccountTransactionWrapper);
                else
                    throw new TimeoutException("Transaction time out. Please try again...");
            }
            finally
            {
                transactionLock.Release();
            }
        }
    }
}