using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFinance.Models.AccountSetup
{
    public class GroupType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("AccountType")]
        public int AccountTypeId { get; set; }
        public virtual AccountType AccountType { get; set; }
        [Required]
        public string Name { get; set; }

        public string? NepaliName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EntryDate { get; set; }
        public int? Schedule { get; set; }
        [Required]
        public string CharKhataNumber { get; set; }
        // [Required]
        // public virtual DebitOrCredit DebitOrCredit { get; set; }
        // public int DebitOrCreditId { get; set; }
        public virtual ICollection<Ledger> Ledgers { get; set; }
        
    }
}