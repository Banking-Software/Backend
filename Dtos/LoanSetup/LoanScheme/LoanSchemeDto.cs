namespace MicroFinance.Dtos.LoanSetup;

public class LoanSchemeDto : CreateLoanSchemeDto
{
    public int Id { get; set; }
    public int AssetsAccountLedgerId { get; set; }
    public int InterestAccountLedgerId { get; set; }
    public string CreatedBy { get; set; }
    public string CreatorId { get; set; }
    public string BranchCode { get; set; }
    public DateTime EnglishCreationDate { get; set; }
    public string NepaliCreationDate { get; set; }
    public DateTime RealWorldCreationDate { get; set; }

    public string? ModifiedBy { get; set; }
    public string? ModifierId { get; set; }
    public string? ModifierBranchCode { get; set; }
    public DateTime? EnglishModificationDate { get; set; }
    public string? NepaliModificationDate { get; set; }
    public DateTime? RealWorldModificationDate { get; set; }

}