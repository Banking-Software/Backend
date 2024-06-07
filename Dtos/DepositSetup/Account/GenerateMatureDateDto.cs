using MicroFinance.Enums;

namespace MicroFinance.Dtos.DepositSetup.Account
{
    public class GenerateMatureDateDto
    {
        public string? OpeningDate { get; set; }
        public DateTime? OpeningDateEnglish { get; set; }
        public PeriodTypeEnum PeriodType { get; set; }
        public int Period { get; set; }
    }
}