using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.CompanyProfile
{
    public class UpdateCalenderDto : IValidatableObject
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int Month { get; set; }
        [Required]
        public string MonthName { get; set; }
        [Required]
        public int NumberOfDay { get; set; }
        public int? RunningDay { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RunningDay.HasValue && RunningDay.Value > NumberOfDay)
            {
                yield return new ValidationResult("RunningDay must be less than or equal to NumberOfDay.", new[] { nameof(RunningDay) });
            }
            if(Month>12)
            {
                yield return new ValidationResult("Month cannot be greater than 12.", new[] { nameof(Month) });
            }
            if(NumberOfDay>32)
            {
                yield return new ValidationResult("Number Of Day cannot be greater than 32.", new[] { nameof(NumberOfDay) });
            }
        }
    }
}