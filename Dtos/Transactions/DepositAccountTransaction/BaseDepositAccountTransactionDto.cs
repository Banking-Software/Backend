using System.ComponentModel.DataAnnotations;
using MicroFinance.Enums.Transaction;

namespace MicroFinance.Dtos.Transactions
{
    public class BaseDepositAccountTransactionDto : IValidatableObject
    {
        [Required]
        public decimal TransactionAmount { get; set; }
        public string? AmountInWords { get; set; }
        [Required]
        public int DepositAccountId { get; set; }
        [Required]
        public PaymentTypeEnum PaymentType { get; set; }
        public int? BankDetailId { get; set; }
        public string? BankChequeNumber { get; set; }
        public int? CollectedByEmployeeId { get; set; }
        public string? Narration { get; set; }
        [Required]
        public string Source { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(PaymentType == PaymentTypeEnum.Bank && BankDetailId==null)
            {
                yield return new ValidationResult("Please Provide the bank details, if Payment type is Bank");
            }
            if(PaymentType==PaymentTypeEnum.Cash && (BankDetailId!=null || BankChequeNumber!=null))
            {
                yield return new ValidationResult("Please remove bank details in case of Cash Payment");
            }
            if(TransactionAmount<0)
            {
                yield return new ValidationResult("Cannot allowed negative transaction");
            }
            if(PaymentType==PaymentTypeEnum.Account)
            {
                yield return new ValidationResult("Invalid Payment Type");
            }
        }
    }
}