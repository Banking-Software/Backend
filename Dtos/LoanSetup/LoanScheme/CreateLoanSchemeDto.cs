using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.LoanSetup;

public class CreateLoanSchemeDto
{
    [Required]
    public string Name { get; set; }
    public string? NepaliName { get; set; }
    [Required]
    public string AliasCode { get; set; }
    [Required]
    public decimal InterestRate { get; set; }
    [Required]
    public decimal MinimumInterestRate { get; set; }
    [Required]
    public decimal MaximumInterestRate { get; set; }
    [Required]
    public bool IsRevolving { get; set; } = false;
    public string? AssetsAccountLedgerName { get; set; }
    public string? InterestAccountLedgerName { get; set; }
    public decimal? PenalInterest { get; set; }
    public decimal? InterestOnInterest { get; set; }
    public decimal? LoanInterestReceivable { get; set; }
    public decimal? OverDueInterest { get; set; }
}