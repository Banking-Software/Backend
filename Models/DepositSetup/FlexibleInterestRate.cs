using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFinance.Models.DepositSetup  
{
    public class FlexibleInterestRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int DepositSchemeId { get; set; }
        [Required]
        public virtual DepositScheme DepositScheme { get; set; }
        [Required]
        public DateTime EffectiveDate { get; set; }
        [Required]
        public int FromAmount { get; set; }
        [Required]
        public int ToAmount { get; set; }
        [Required]
        [Range(0, 100)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid decimal value")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal InterestRate { get; set; }
    }
}