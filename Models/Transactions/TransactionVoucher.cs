namespace MicroFinance.Models.Transactions
{
    public class TransactionVoucher
    {
        public int Id { get; set; }
        public string VoucherNumber { get; set; }
        public virtual ICollection<BaseTransaction> BaseTransactions { get; set; }
    }
}