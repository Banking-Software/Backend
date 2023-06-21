using System.ComponentModel.DataAnnotations;
using MicroFinance.Models.DepositSetup;

namespace MicroFinance.Models.Transactions  
{
    public class Transaction
    {
        public int Id { get; set; }

        public int DepositAccountId { get; set; }
        public DepositAccount DepositAccount { get; set; }
        [Required]
        public int TransactionType { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }

        public virtual DepositTransaction DepositTransaction { get; set; }
    }
}