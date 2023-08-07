using MicroFinance.Enums.Transaction;
using MicroFinance.Models.AccountSetup;

namespace MicroFinance.Models.Transactions
{
    public class SubLedgerTransaction
    {
       public int Id { get; set; }
        public virtual BaseTransaction Transaction { get; set; }
        public int TransactionId { get; set; }
        public virtual SubLedger SubLedger { get; set; }
        public int SubLedgerId { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public string? Remarks { get; set; }
        public decimal BalanceAfterTransaction { get; set; }
    }
}