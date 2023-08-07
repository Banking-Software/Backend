using MicroFinance.Enums;
using MicroFinance.Enums.Transaction;

namespace MicroFinance.Models.Wrapper   
{
    public class TransactionProcessDetail
    {
        public TransactionRemarks TransactionRemarks { get; set; }
        public string AccountNumber { get; set; }
        public decimal TransactionAmount { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }

    }
}