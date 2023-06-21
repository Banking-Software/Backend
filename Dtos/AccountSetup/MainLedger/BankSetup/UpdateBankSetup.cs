using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.AccountSetup.MainLedger
{
    public class UpdateBankSetup
    {
        public int Id { get; set; }
        public string NepaliName { get; set; }
        public string BankBranch { get; set; }
        public int BankTypeId { get; set; }
        [Range(0, 100, ErrorMessage = "Interest Rate must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest Rate must have up to two decimal places.")]
        public decimal InterestRate { get; set; }
    }
}