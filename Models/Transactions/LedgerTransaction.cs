using MicroFinance.Enums.Transaction;
using MicroFinance.Models.AccountSetup;

namespace MicroFinance.Models.Transactions
{
    public class LedgerTransaction
    {
        public int Id { get; set; }
        public virtual BaseTransaction Transaction { get; set; }
        public int TransactionId { get; set; }
        public virtual Ledger Ledger { get; set; }
        public int? LedgerId { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public string? Remarks { get; set; }
        public decimal BalanceAfterTransaction { get; set; }
    }
}