using MicroFinance.Models.DepositSetup;

namespace MicroFinance.Models.Transactions
{
    public class InterestTransaction
    {
        public int Id { get; set; }
        public decimal CalculatedInterestRate { get; set; }
        public decimal CalculatedInterestAmount { get; set; }
        public bool IsInterestPosted { get; set; }
        public virtual DepositAccount DepositAccount {get; set;}
        public int DepositAccountId { get; set; }
        public virtual BaseTransaction Transaction { get; set; }
        public int TransactionId { get; set; }
    }
}