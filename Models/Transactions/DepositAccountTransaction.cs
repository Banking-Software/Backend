
using MicroFinance.Enums.Transaction;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.DepositSetup;

namespace MicroFinance.Models.Transactions
{
    public class DepositAccountTransaction
    {
        public int Id { get; set; }
        public virtual BaseTransaction Transaction { get; set; }
        public int TransactionId { get; set; }
        public virtual DepositAccount DepositAccount { get; set; }
        public int? DepositAccountId { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public WithDrawalTypeEnum? WithDrawalType { get; set; } 
        public string? WithDrawalChequeNumber { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
        public BankSetup? BankDetail { get; set; }
        public int? BankDetailId { get; set; }
        public string? BankChequeNumber { get; set; }
        public int? CollectedByEmployeeId { get; set; }
        public string? Narration { get; set; }
        public string Source { get; set; }
        public string? Remarks { get; set; }
        public decimal BalanceAfterTransaction { get; set; }
    }
}