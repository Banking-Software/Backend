using MicroFinance.Enums;
using MicroFinance.Models.ClientSetup;

namespace MicroFinance.Models.LoanSetup;

public class LoanAccount : BaseModel
{
    public int Id { get; set; }
    public virtual LoanScheme LoanScheme { get; set; }
    public int? LoanSchemeId { get; set; }
    public string AccountNumber { get; set; }
    public string Sector { get; set; }
    public virtual Client Client { get; set; }
    public int? ClientId { get; set; }
    public LoanInterestTypeEnum InterestType { get; set; }
    public PeriodTypeEnum PeriodType { get; set; } // For Maturity
    public int Period { get; set; } // For Maturity
    public DateTime MatureDate { get; set; }
    public decimal InterestRate { get; set; }
    public decimal LoanLimit { get; set; }
    public int ReferredByEmployeeId { get; set; }
    public List<int>? WithDrawalBlockedDepositAccountIds { get; set; }
    public AccountStatusEnum Status { get; set; } // No Suspended, exclude this
    public string? Shakshi1 { get; set; }
    public string? Shakshi2 { get; set; }
    public string? Jamani1 { get; set; }
    public string? Jamani2 { get; set; }

    // Security and Insurance
    public string? SecurityType { get; set; }
    public string? SecurityValue { get; set; }
    public string? OwnerName { get; set; }
    public string? PurjaNo { get; set; }
    public string? KittaNo { get; set; }
    public string? RokkaNo { get; set; }
    public string? VechileNo { get; set; }
    public string? TaxPaidDate { get; set; }
    public string? LandArea { get; set; }
    public string? RokkaMiti { get; set; }
    public string? VatNo { get; set; }
    public string? PanNo { get; set; }
    public string? CompanyName { get; set; }
    public string? Amount { get; set; }
    public string? PolicyNo { get; set; }
    public string? StartDate { get; set; }
    public string? ExpiryDate { get; set; }

    public byte[]? UploadedDocument { get; set; }
    public FileType? UploadedDocumentType { get; set; }
    public string? UploadedDocumentFileName { get; set; }

    // Schedule
    public LoanScheduleEnum? ScheduleType { get; set; }
    public LoanScheduleEnum? InterestPaymentType { get; set; }
    public int? GracePeriod { get; set; }
}