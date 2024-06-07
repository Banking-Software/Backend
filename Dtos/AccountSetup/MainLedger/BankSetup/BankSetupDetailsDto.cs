namespace MicroFinance.Dtos.AccountSetup.MainLedger
{
    public class BankSetupDetailsDto
    {
        public AccountTypeDto AccountType { get; set; }
        public GroupTypeDto GroupType { get; set; }
        public BankSetupDto BankSetup { get; set; }
        public LedgerDto Ledger { get; set; }
    }
}