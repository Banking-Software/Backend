using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFinance.Models.AccountSetup
{
    public class GroupTypeDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        public string? NepaliName { get; set; }
        [Required]
        public string BankBranch { get; set; }
        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public string BankType { get; set; }
        [Column(TypeName ="decimal(5,2)")]
        public decimal? InterestRate { get; set; }
        [Required]
        public string Branch { get; set; }
        public int GroupTypeId { get; set; }
        public GroupType GroupType { get; set; }
    }
}