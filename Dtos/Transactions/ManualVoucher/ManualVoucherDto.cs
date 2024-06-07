using System.ComponentModel.DataAnnotations;
using MicroFinance.Enums.Transaction;

namespace MicroFinance.Dto.Transactions
{
    public class ManualVoucherDto
    {
        [Required]
        public bool IsLedger { get; set; } // Ledger - 1, Subledger - 0
        [Required]
        public int LedgerdId { get; set; } // Ledger or Subledger Id
        [Required]
        public TransactionTypeEnum TransactionType { get; set; } // Debit - 1, Credit - 2
        [Required]
        public decimal TransactionAmount { get; set; }
        public string? Description { get; set; }
    }
}