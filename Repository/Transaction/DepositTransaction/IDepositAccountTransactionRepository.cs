using MicroFinance.Enums.Transaction;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper.TrasactionWrapper;

namespace MicroFinance.Repository.Transaction
{
    public interface IDepositAccountTransactionRepository
    {
        // Task<string> MakeDeposit(DepositAccountTransactionWrapper depositWrapper);
        // Task<string> MakeWithDrawal(DepositAccountTransactionWrapper withDrawalWrapper);
        Task<string> MakeTransaction(DepositAccountTransactionWrapper transactionData);
        Task<BaseTransaction> BaseTransaction(BaseTransaction baseTransaction, PaymentTypeEnum paymentType, int? bankDetailId, string? bankChequeNumber);
        Task<DepositAccount> BaseTransactionOnDepositAccount(DepositAccountTransactionWrapper depositAccountTransactionWrapper, BaseTransaction baseTransaction);
        Task BaseTransactionOnLedger(BaseTransaction baseTransaction, PaymentTypeEnum paymentType, TransactionTypeEnum ledgerTransactionType, bool isDeposit);
    }
}