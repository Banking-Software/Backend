using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.AccountSetup.MainLedger
{
    public class UpdateBankSetup
    {
        [Required]
        public int Id { get; set;}
        public string NepaliName { get; set; }
        [Required]
        public string BankBranch { get; set; }
        [Required]
        [RegularExpression(@"^[1-3]", ErrorMessage = "Please Enter Correct Bank Type")]
        public int BankTypeId { get; set; }
        [Range(0, 100, ErrorMessage = "Interest Rate must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest Rate must have up to two decimal places.")]
        [Required]
        public decimal InterestRate { get; set; }
    }
}