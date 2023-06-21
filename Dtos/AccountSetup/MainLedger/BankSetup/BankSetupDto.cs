using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.AccountSetup.MainLedger
{
    public class BankSetupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? NepaliName { get; set; }
        public string BankBranch { get; set; }
        public string AccountNumber { get; set; }
        public int BankTypeId { get; set; }
        public string BankType { get; set; }
        public decimal? InterestRate { get; set; }
        public string BranchCode { get; set; }
    }
}