using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.CompanyProfile
{
    public class CreateCalenderDto : IValidatableObject
    {
        [Required]
        public int Year { get; set; }
        [Required]
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
        public int Month { get; set; }
        [Required]
        public string MonthName { get; set; }
        [Required]
        [Range(1, 32, ErrorMessage = "Number Of Day must be between 1 and 32.")]
        public int NumberOfDay { get; set; }
        public int? RunningDay { get; set; }
        [Required]
        public bool IsActive { get; set; }

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

    public class CalendarServiceDto
    {
        public void ValidateCalenderList(List<CreateCalenderDto> calenderList)
        {
            
            if(calenderList.Count<12 || calenderList.Count>12)
                throw new ValidationException($" {calenderList.Count} calender is requested to create but exact 12 months calender need to requested.");

            int year = calenderList[0].Year;
            var falseYear= calenderList.Where(c=>c.Year!=year).FirstOrDefault();

            if(falseYear!=null) 
                throw new ValidationException("Year should be same for all the months");

            int activeCalendarCount=  calenderList.Count(c=>c.IsActive==true);
            if(activeCalendarCount>1)
            {
                throw new ValidationException("Only one month can be active at a time");
            }
        }
    }
}