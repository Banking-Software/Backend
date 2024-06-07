using MicroFinance.Models.ClientSetup;
using MicroFinance.Models.DepositSetup;

namespace MicroFinance.Models.DepositSetup
{
    public class JointAccount
    {
        public int Id { get; set; }
        public int? DepositAccountId { get; set; }
        public virtual DepositAccount DepositAccount { get; set; }
        public int? JointClientId { get; set; }
        public virtual Client JointClient { get; set; }
        public DateTime RealWorldStartDate { get; set; }
        public DateTime? RealWorldEndDate { get; set; }
        public string CompanyCalendarStartDate { get; set; }
        public string? CompanyCalendarEndDate { get; set; }

    }
}