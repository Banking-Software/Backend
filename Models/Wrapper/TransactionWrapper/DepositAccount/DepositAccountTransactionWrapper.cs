using MicroFinance.Enums.Transaction;
using MicroFinance.Models.AccountSetup;

namespace MicroFinance.Models.Wrapper.TrasactionWrapper
{
    public class DepositAccountTransactionWrapper : BaseTransactionWrapper
    {
        public decimal TransactionAmount { get; set; }
        public string? AmountInWords { get; set; }
        public int DepositAccountId { get; set; }
        public int DepositSchemeId { get; set; }
        public int DepositSchemeSubLedgerId { get; set; }
        public int DepositSchemeLedgerId {get; set;}
        public int? BankLedgerId { get; set; }
        public string AccountNumber { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
        public int? BankDetailId { get; set; }
        public string? BankChequeNumber { get; set; }
        public int? CollectedByEmployeeId { get; set; }
        public string? Narration { get; set; }
        public string Source { get; set; }
        // Extra For WithDrawal
        public WithDrawalTypeEnum? WithDrawalType { get; set; } 
        public string? WithDrawalChequeNumber { get; set; }
    }
}