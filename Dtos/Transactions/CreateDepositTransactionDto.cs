namespace MicroFinance.Dtos.Transactions
{
    public class CreateDepositTransactionDto
    {
        public int DepositAccountId { get; set; }
        public int PaymentType { get; set; }
        public int TransactionAmount { get; set; }
        public string? Employee { get; set; }
        public string? Narration { get; set; }
        public string Source { get; set; }
        public int? OpeningCharge { get; set; }
    }
}