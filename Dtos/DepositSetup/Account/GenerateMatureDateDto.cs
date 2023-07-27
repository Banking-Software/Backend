using MicroFinance.Enums.Deposit.Account;

namespace MicroFinance.Dtos.DepositSetup.Account
{
    public class GenerateMatureDateDto
    {
        public string OpeningDate { get; set; }
        public PeriodTypeEnum PeriodType { get; set; }
        public int Period { get; set; }
    }
}