using MicroFinance.Enums.Transaction;

namespace MicroFinance.Dtos.Reports;

public class BaseReportDto
{
    public int BaseTransactionId { get; set; }
    public string VoucherNumber { get; set; }
    public string? TransactionRemarks { get; set; }
    public TransactionTypeEnum TransactionType { get; set; }
    public decimal  TransactionAmount { get; set; }
    public decimal BalanceAfterTransaction { get; set; }
    public DateTime EnglishCreationDate { get; set; }
    public string NepaliCreationDate { get; set; }
    public DateTime RealWorldCreationDate { get; set; }
    public string Narration { get; set; }
    public string? Description { get; set; }
    public string TransactionDoneBy { get; set; }
}