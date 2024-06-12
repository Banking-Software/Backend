using MicroFinance.Enums;
using MicroFinance.Enums.Transaction;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Transactions;

namespace MicroFinance.Models.Wrapper.DayEnd    
{
    public class InterestPostWrapper
    {
        public DepositAccount FromAccount { get; set; }
        public bool isPostingToSameAccount {get; set;}
        public DepositAccount? ToAccount { get; set; }
        public SubLedger? DepositSchemeInterestSubLedger { get; set; }
        public Ledger? DepositSchemeInterestLedger { get; set; }
        public SubLedger? DepositSchemeTaxSubLedger { get; set; }
        public Ledger? DepositSchemeTaxLedger { get; set; }
        public TransactionTypeEnum AccountTransactionType { get; set; }
        public TransactionTypeEnum SubLedgerTransactionType { get; set; }
        public BaseTransaction baseTransaction { get; set; }
        public TypeOfDayEndTaskEnum typeOfDayEndTask {get; set;}
    }
}