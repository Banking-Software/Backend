using MicroFinance.Models.Wrapper.TrasactionWrapper;

namespace MicroFinance.Repository.Transaction
{
    public interface IShareAccountTransactionRepository
    {
        Task<string> LockAndMakeShareTransaction(ShareAccountTransactionWrapper shareAccountTransactionWrapper);
    }
}