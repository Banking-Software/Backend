using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroFinance.Models.ClientSetup;

namespace MicroFinance.Models.DepositSetup
{
    public class DepositAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int DepositSchemeId { get; set; }
        [Required]
        public virtual DepositScheme DepositScheme { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public int ClientId { get; set; }
        [Required]
        public virtual Client Client { get; set; }
        [Required]
        public DateTime OpeningDate { get; set; }
        public int? Period { get; set; }
        public int? PeriodType { get; set; }
        [Required]
        public int AccountType { get; set; }

        public int? JointClientId { get; set; }
        public virtual Client? JointClient { get; set; }
        public DateTime? MatureDate { get; set; }
        [Required]
        [Range(0, 100)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid decimal value")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal InterestRate { get; set; }
        [Required]
        public int MinimumBalance { get; set; } // May or maynot be same as that of Deposit Scheme MinimumBalance.
        [Column(TypeName = "decimal(18,4)")]
        public decimal PrincipalAmount { get; set; } = 0;
        [Column(TypeName = "decimal(18,4)")]
        public decimal InterestAmount { get; set; } = 0;
        [Required]
        public string ReferredBy { get; set; }
        [Required]
        public string InterestPostingAccountNumber { get; set; }
        public string? MatureInterestPostingAccountNumber { get; set; }
        public string? Description { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public bool IsSMSServiceActive { get; set; }
        public int? DailyDepositAmount { get; set; }
        public int? TotalDepositDay { get; set; }
        public int? TotalDepositAmount { get; set; }
        public int? TotalReturnAmount { get; set; }
        public int? TotalInterestAmount { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        

    }
}