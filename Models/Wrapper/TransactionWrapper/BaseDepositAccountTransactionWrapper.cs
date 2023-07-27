using MicroFinance.Enums.Transaction;
using MicroFinance.Models.AccountSetup;

namespace MicroFinance.Models.Wrapper.TrasactionWrapper
{
    public class BaseDepositAccountTransactionWrapper
    {
        // Basic Info
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string CreatorId { get; set; }
        public string BranchCode { get; set; }
        public DateTime RealWorldCreationDate { get; set; }
        public string CompanyCalendarCreationDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifierId { get; set; }
        public string? ModifierBranchCode { get; set; }
        public DateTime? RealWorldModificationDate { get; set; }
        public string? CompanyCalendarModificationDate { get; set; }
        //
        public decimal TransactionAmount { get; set; }
        public string? AmountInWords { get; set; }
        public int DepositAccountId { get; set; }
        public int DepositSchemeId { get; set; }
        public int DepositSchemeSubLedgerId { get; set; }
        public int? BankLedgerId { get; set; }
        public string AccountNumber { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
        public int? BankDetailId { get; set; }
        public string? BankChequeNumber { get; set; }
        public int? CollectedByEmployeeId { get; set; }
        public string? Narration { get; set; }
        public string Source { get; set; }
    }
}