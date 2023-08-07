using MicroFinance.Enums.Transaction;
using MicroFinance.Enums.Transaction.ShareTransaction;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Share;

namespace MicroFinance.Models.Transactions
{
    public class ShareTransaction
    {
        public int Id { get; set; }
        public ShareTransactionTypeEnum ShareTransactionType { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public string? ShareCertificateNumber { get; set; }
        public string? Narration { get; set; }
        public string? Remarks { get; set; }

        public virtual BaseTransaction Transaction { get; set; }
        public int TransactionId { get; set; }
        public virtual ShareAccount ShareAccount { get; set; }
        public int? ShareAccountId { get; set; }
        public virtual DepositAccount TransferToAccount { get; set; }
        public int? TransferToDepositAccountId { get; set; }
        public virtual DepositAccount PaymentDepositAccount { get; set; }
        public int? PaymentDepositAccountId { get; set; }
        public decimal BalanceAfterTransaction{ get; set; }
    }
}