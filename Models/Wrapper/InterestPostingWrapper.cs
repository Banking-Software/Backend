using MicroFinance.Enums.Transaction;

namespace MicroFinance.Models.Wrapper;

public class InterestPostingWrapper
{
    public int FromAccountId { get; set; }
    public int ToAccountId { get; set; }
    public string FromAccountNumber { get; set; }
    public string ToAccountNumber { get; set; }
    public int SchemeId { get; set; }
    public int? TaxSubLedgerId { get; set; }
    public int? InterestSubLedgerId { get; set; }
    public decimal TransactionAmount { get; set; }
    public DateTime TransactionDate { get; set; }
    public TransactionTypeEnum TransactionType { get; set; }
}