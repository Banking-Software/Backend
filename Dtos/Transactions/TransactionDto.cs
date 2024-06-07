namespace MicroFinance.Dtos.Transactions
{
    public class TransactionDto : TransactionBasicInfoDto
    {
        public string VoucherNumber { get; set; }
        public decimal TransactionAmount { get; set; }
        public string? AmountInWords { get; set; }
        public string? Remarks { get; set; }
    }
}