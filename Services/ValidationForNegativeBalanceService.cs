using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Services
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidationForNegativeBalanceService : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value is decimal balance && balance < 0)
            {
                return new ValidationResult("Balance Cannot be negative.");
            }
           return ValidationResult.Success;
        }
        
    }
}