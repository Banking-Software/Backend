using System.ComponentModel.DataAnnotations;
using MicroFinance.Enums;

namespace MicroFinance.Dtos.LoanSetup;

public class GenerateLoanScheduleDto : IValidatableObject
{
    public int? LoanSchemeId { get; set; }
    public PostingSchemeEnum InterestPaymentSchedule { get; set; } // ONLY MONTHLY, EMI And NONE exist
    public PostingSchemeEnum PrincipalPaymentSchedule { get; set; }
    public PeriodTypeEnum PeriodType { get; set; }
    public int Period  { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal LoanLimit { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(InterestPaymentSchedule!=PostingSchemeEnum.Monthly && InterestPaymentSchedule!=PostingSchemeEnum.EMI && InterestPaymentSchedule!=PostingSchemeEnum.None)
        {
            yield return new ValidationResult("Only Monthly, EMI, None is valid loan interest schedule");
        }
        if((InterestPaymentSchedule==PostingSchemeEnum.EMI && PrincipalPaymentSchedule!=PostingSchemeEnum.EMI) || (InterestPaymentSchedule!=PostingSchemeEnum.EMI && PrincipalPaymentSchedule==PostingSchemeEnum.EMI))
        {
            yield return new ValidationResult("In case of EMI both Interest and Principal amount schedule should be EMI");
        }
        
    }
}