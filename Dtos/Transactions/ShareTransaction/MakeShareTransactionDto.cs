using System.ComponentModel.DataAnnotations;
using MicroFinance.Enums.Transaction;
using MicroFinance.Enums.Transaction.ShareTransaction;

namespace MicroFinance.Dtos.Transactions.ShareTransaction
{
    public class MakeShareTransactionDto : IValidatableObject
    {
        public decimal TransactionAmount { get; set; }
        public string? AmountInWords { get; set; }
        public string? ShareCertificateNumber { get; set; }
        public string? Narration { get; set; }
        public int ShareAccountId { get; set; }
        public int ClientId { get; set; }
        public ShareTransactionTypeEnum ShareTransactionType { get; set; }
        public int? TransferToDepositAccountId { get; set; }
        public PaymentTypeEnum? PaymentType { get; set; }
        public int? BankDetailId { get; set; }
        public string? BankChequeNumber { get; set; }
        public int? PaymentDepositAccountId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if((ShareTransactionType == ShareTransactionTypeEnum.Issue || ShareTransactionType == ShareTransactionTypeEnum.Refund) && TransferToDepositAccountId!=null)
            {
                yield return new ValidationResult("Incase of Issue and Refund donot attach the Transfer Account Details");
            }
            if(ShareTransactionType == ShareTransactionTypeEnum.Transfer && (PaymentType!=null || TransferToDepositAccountId==null))
            {
                yield return new ValidationResult("No payment method is accepted and one deposit account is required in case of Transfer transaction");
            }
            if(ShareTransactionType != ShareTransactionTypeEnum.Transfer && PaymentType==null)
            {
                yield return new ValidationResult("Please provide atleast one payment method");
            }
            if(PaymentType==PaymentTypeEnum.Cash && (BankDetailId!=null || BankChequeNumber!=null || PaymentDepositAccountId!=null))
            {
                yield return new ValidationResult("Bank and Deposit Account details not allowed in case of cash transaction");
            }
            if(PaymentType==PaymentTypeEnum.Bank && (BankDetailId==null || BankChequeNumber==null || PaymentDepositAccountId!=null))
            {
                yield return new ValidationResult("Bank Detail is required and Deposit Account is not allowed in case of banking transaction");
            }
            if(PaymentType==PaymentTypeEnum.Account && (BankDetailId!=null || BankChequeNumber!=null || PaymentDepositAccountId==null))
            {
                yield return new ValidationResult("Deposit Account is required and other details like: 'Bank, Cheque Number' are not allowed");
            }
        }
    }
}