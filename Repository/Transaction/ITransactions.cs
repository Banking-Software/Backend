using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Share;
using MicroFinance.Models.Transactions;

namespace MicroFinance.Repository.Transaction;

public interface ITransactions
{
    Task <TransactionVoucher> GenerateTransactionVoucher(string branchCode, int voucherIndex);
    Task GenerateBaseTransaction(BaseTransaction baseTransaction);

    Task TransactionOnDepositAccount(DepositAccountTransaction depositAccountTransaction, bool isDeposit);
    Task TransactionOnShareAccount(ShareAccount shareAccount, decimal transactionAmount, bool isDeposit);
    Task TransactionOnLedger(LedgerTransaction ledgerTransaction, bool isDeposit);
    Task TransactionOnSubLedger(SubLedgerTransaction subLedgerTransaction, bool isDeposit);

    Task TransactionEntryForDepositAccount(DepositAccountTransaction depositAccountTransaction);
    Task TransactionEntryForShareAccount(ShareTransaction shareTransaction);
    Task TransactionEntryForLedger(LedgerTransaction ledgerTransaction);
    Task TransactionEntriesForLedger(List<LedgerTransaction> ledgerTransactions);
    Task TransactionEntryForSubLedger(SubLedgerTransaction subLedgerTransaction);
}