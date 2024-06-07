using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.AccountSetup.MainLedger
{
    public class CreateLedgerDto
    {
        // [RegularExpression(@"^(?!0$)\d+$", ErrorMessage = "Id must be a non-zero positive integer.")]
        // public int Id { get; set; }
        [Required]
        public int GroupTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        public string? NepaliName { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime EntryDate { get; set; }
        [Required]
        public bool IsSubLedgerActive { get; set; }
        
        [Range(0, 100, ErrorMessage = "DepreciationRate must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "DepreciationRate must have up to two decimal places.")]
        public decimal? DepreciationRate { get; set; }
        public string? HisabNumber { get; set; }
    }
}