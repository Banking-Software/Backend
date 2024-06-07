using MicroFinance.Enums.Transaction;
using MicroFinance.Models.AccountSetup;

namespace MicroFinance.Models.Transactions  
{
    public class BaseTransaction : TransactionBasicInfo
    {
        public decimal TransactionAmount { get; set; }
        public string? AmountInWords { get; set; }
        public string? Remarks { get; set; }
        public PaymentTypeEnum? PaymentType { get; set; }
        public virtual BankSetup? BankDetail { get; set; }
        public int? BankDetailId { get; set; }
        public string? BankChequeNumber { get; set; }
        public virtual TransactionVoucher TransactionVoucher { get; set; }
        public int VoucherId { get; set; }
        public virtual DepositAccountTransaction DepositAccountTransaction { get; set; }
        public virtual ICollection<LedgerTransaction> LedgerTransaction { get; set; }
        public virtual SubLedgerTransaction SubLedgerTransaction { get; set; }
        public virtual ShareTransaction ShareTransaction { get; set; }
        public virtual InterestTransaction InterestTransactions { get; set; }
    }
}