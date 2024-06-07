using MicroFinance.Dtos.ClientSetup;

namespace MicroFinance.Dtos.DepositSetup.Account
{
    public class JointAccountDto
    {
        public int Id { get; set; }
        public ClientDto JointClient { get; set; }
        public DateTime RealWorldStartDate { get; set; }
        public DateTime? RealWorldEndDate { get; set; }
        public string CompanyCalendarStartDate { get; set; }
        public string? CompanyCalendarEndDate { get; set; }
    }
}