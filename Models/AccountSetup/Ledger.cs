using System.ComponentModel.DataAnnotations;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.LoanSetup;
using MicroFinance.Models.Transactions;

namespace MicroFinance.Models.AccountSetup
{
    public class Ledger
    {
        public int Id { get; set; }
        public int? LedgerCode { get; set; }
        public virtual GroupType GroupType { get; set; }
        public int GroupTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        public string? NepaliName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EntryDate { get; set; }
        public decimal? DepreciationRate { get; set; }
        public string? HisabNumber { get; set; }
        [Required]
        public bool IsSubLedgerActive { get; set; } // If True then allow to create Sub Ledger
        // [ValidationForNegativeBalanceService]
        public decimal CurrentBalance { get; set; }=0;        
        [Required]
        public bool IsBank { get; set; }
        public virtual ICollection<DepositScheme> DepositSchemes { get; set; }
        public virtual LoanScheme AssetsLoanScheme { get; set; }
        public virtual LoanScheme InterestLoanSchme { get; set; }
        
        public virtual BankSetup BankSetup{ get; set; }
        public virtual ICollection<SubLedger> SubLedger { get; set; } 
        public virtual ICollection<Client> Client {get; set;}
        public virtual ICollection<LedgerTransaction> LedgerTransactions { get; set; }
    }
}