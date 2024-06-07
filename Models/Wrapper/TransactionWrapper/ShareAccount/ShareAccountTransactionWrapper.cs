using MicroFinance.Enums.Transaction;
using MicroFinance.Enums.Transaction.ShareTransaction;

namespace MicroFinance.Models.Wrapper.TrasactionWrapper
{
    public class ShareAccountTransactionWrapper : BaseTransactionWrapper
    {
        public decimal TransactionAmount { get; set; }
        public string? AmountInWords { get; set; }
        public string? ShareCertificateNumber { get; set; }
        public string? Narration { get; set; }
        public int ShareAccountId { get; set; }
        public int ShareKittaId { get; set; }
        public int ClientId { get; set; }
        public ShareTransactionTypeEnum ShareTransactionType { get; set; }
        public int? TransferToDepositAccountId { get; set; }
        public int? TransferToDepositSchemeId { get; set; }
        public int? TransferToDepositSchemeSubLedgerId { get; set; }
        public int? TransferToDepositSchemeLedgerId { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
        public int? BankDetailId { get; set; }
        public string? BankChequeNumber { get; set; }
        public int? BankLedgerId { get; set; }
        public int? PaymentDepositAccountId { get; set; }
        public int? PaymentDepositSchemeId { get; set; }
        public int? PaymentDepositSchemeSubLedgerId { get; set; }
        public int? PaymentDepositSchemeLedgerId { get; set; }
    }
}