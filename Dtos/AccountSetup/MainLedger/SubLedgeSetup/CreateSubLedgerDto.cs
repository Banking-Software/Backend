using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.AccountSetup.MainLedger
{
    public class CreateSubLedgerDto
    {
        // [Required]
        // [RegularExpression(@"^(?!0$)\d+$", ErrorMessage = "Id must be a non-zero positive integer.")]
        // public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public int LedgerId { get; set; }
    }
}