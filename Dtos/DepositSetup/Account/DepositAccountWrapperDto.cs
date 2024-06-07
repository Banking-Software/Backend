using MicroFinance.Dtos.ClientSetup;

namespace MicroFinance.Dtos.DepositSetup.Account
{
    public class DepositAccountWrapperDto
    {
        public DepositAccountDto DepositAccount { get; set; }
        public DepositSchemeDto DepositScheme { get; set; }
        public ClientDto Client { get; set; }
        public List<JointAccountDto>? JointClients { get; set; }
    }
}