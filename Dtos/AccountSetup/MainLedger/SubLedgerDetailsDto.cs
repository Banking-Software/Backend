namespace MicroFinance.Dtos.AccountSetup.MainLedger
{
    public class SubLedgerDetailsDto
    {
        public AccountTypeDto AccountType { get; set; }
        public GroupTypeDto GroupType { get; set; }
        public LedgerDto Ledger { get; set; }
        public SubLedgerDto SubLedger { get; set; }
    }
}