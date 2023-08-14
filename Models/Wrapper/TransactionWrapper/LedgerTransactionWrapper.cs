using MicroFinance.Enums.Transaction;
using MicroFinance.Models.Transactions;

namespace MicroFinance.Models.Wrapper.TrasactionWrapper;

public class LedgerTransactionWrapper
{
    public BaseTransaction BaseTransaction { get; set; }
    public TransactionTypeEnum LedgerTransactionType { get; set; }
    public PaymentTypeEnum PaymentType { get; set; }
    public bool IsDeposit { get; set; }
    public string? ledgerRemarks { get; set; }
    public string? LedgerNarration { get; set; }

}