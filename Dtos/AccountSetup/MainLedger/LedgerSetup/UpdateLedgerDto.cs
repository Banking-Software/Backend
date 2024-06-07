using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.AccountSetup.MainLedger
{
    public class UpdateLedgerDto
    {
        [Required]
        public int Id { get; set; }
        public string? NepaliName { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsSubLedgerActive { get; set; }
        [Range(0, 100, ErrorMessage = "Depreciation Rate must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Depreciation Rate must have up to two decimal places.")]
        public decimal? DepreciationRate { get; set; }
        public string? HisabNumber { get; set; }
    }
}