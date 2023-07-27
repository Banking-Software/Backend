using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper.TrasactionWrapper;

namespace MicroFinance.Repository.Transaction
{
    public interface IDepositAccountTransactionRepository
    {
        Task<string> MakeDeposit(MakeDepositWrapper depositWrapper);
        Task<string> MakeWithDrawal(MakeWithDrawalWrapper withDrawalWrapper);
    }
}