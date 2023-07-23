using MicroFinance.Models.DepositSetup;

namespace MicroFinance.Models.Wrapper
{
    public class DepositAccountWrapper
    {
        public DepositAccount DepositAccount { get; set; }
        public List<JointAccount>? JointAccount { get; set; }
    }
}