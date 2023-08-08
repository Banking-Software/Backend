using MicroFinance.Enums.Transaction;

namespace MicroFinance.Models.Wrapper;

public class TransactionReportWrapper
{
    public int BaseTransactionId { get; set; }
    public string VoucherNumber { get; set; }
    public string? TransactionRemarks { get; set; }
    public TransactionTypeEnum TransactionType { get; set; }
    public decimal BalanceAfterTransaction { get; set; }
    public DateTime RealWorldCreationDate { get; set; }
    public string CompanyCalendarCreationDate { get; set; }
    public string Narration { get; set; }
    public string? Description { get; set; }
    public string TransactionDoneBy { get; set; }
}