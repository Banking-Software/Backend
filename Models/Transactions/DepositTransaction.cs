using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFinance.Models.Transactions
{
    public class DepositTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        [Required]
        public int PaymentType { get; set; }
        [Required]
        public int TransactionAmount { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalAmountAfterTransaction { get; set; }
        public string Employee { get; set; }
        [Required]
        public string Source { get; set; }
        public string Narration { get; set; }
        public int OpeningCharge { get; set; }

        
    }
}