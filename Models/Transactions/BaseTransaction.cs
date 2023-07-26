namespace MicroFinance.Models.Transactions  
{
    public class BaseTransaction : TransactionBasicInfo
    {
        public string? VoucherNumber { get; set; }
        public decimal TransactionAmount { get; set; }
        public string? AmountInWords { get; set; }
        public string? Remarks { get; set; }

        public virtual DepositAccountTransaction DepositAccountTransaction { get; set; }
        public virtual LedgerTransaction LedgerTransaction { get; set; }
        public virtual SubLedgerTransaction SubLedgerTransaction { get; set; }
    }
}