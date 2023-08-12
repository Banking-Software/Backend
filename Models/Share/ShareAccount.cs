using MicroFinance.Models.ClientSetup;
using MicroFinance.Models.Transactions;

namespace MicroFinance.Models.Share
{
    public class ShareAccount
    {
        public int Id { get; set; }
        public virtual Client Client { get; set; }
        public int ClientId { get; set; }
        public decimal CurrentShareBalance { get; set; }=0;
        public bool IsActive { get; set; }
        public bool IsClose { get; set; }

        public virtual ICollection<ShareTransaction> ShareTransactions { get; set; }
    }
}