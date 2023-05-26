using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.AccountSetup.MainLedger
{
    public class GroupTypeDetailsDto
    {
        [Required]
        public int Id { get; set; }
        // [Required]
        // public int GroupTypeId { get; set; }
        // [Required]
        // public int AccountTypeId { get; set; }

        [Required]
        public string Name { get; set; }
        // [Required]
        // public string GroupTypeName { get; set; }
        // [Required]
        // public string AccountTypeName { get; set; }

        public string? NepaliName { get; set; }

        [Required]
        public string BankBranch { get; set; }

        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public string BankType { get; set; }

        [Range(0, 100, ErrorMessage = "Interest Rate must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest Rate must have up to two decimal places.")]
        public decimal? InterestRate { get; set; }

        [Required]
        public string Branch { get; set; }
    }
}