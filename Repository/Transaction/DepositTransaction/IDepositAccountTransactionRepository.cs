using MicroFinance.Enums.Transaction;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper.TrasactionWrapper;

namespace MicroFinance.Repository.Transaction
{
    public interface IDepositAccountTransactionRepository
    {
        // Task<string> MakeTransaction(DepositAccountTransactionWrapper transactionData);
        Task<string> HandleDepositAccountTransaction(DepositAccountTransactionWrapper transactionData);
        
    }
}