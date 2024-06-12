using MicroFinance.Models.AccountSetup;

namespace MicroFinance.Models.LoanSetup;

public class LoanScheme : BaseModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? NepaliName { get; set; }
    public string AliasCode { get; set; }
    public decimal InterestRate { get; set; }
    public decimal MinimumInterestRate { get; set; }
    public decimal MaximumInterestRate { get; set; }
    public bool IsRevolving { get; set; } = false;
    public bool IsActive { get; set; } = true;
    // FINE related
    public decimal? PenalInterest { get; set; }
    public decimal? InterestOnInterest { get; set; }
    public decimal? LoanInterestReceivable { get; set; }
    public decimal? OverDueInterest { get; set; }
    // 
    public virtual Ledger AssetsAccountLedger { get; set; }
    public int? AssetsAccountLedgerId { get; set; }
    public virtual Ledger InterestAccountLedger { get; set; }
    public int? InterestAccountLedgerId { get; set; }
    public virtual ICollection<LoanAccount> LoanAccounts { get; set; }
}