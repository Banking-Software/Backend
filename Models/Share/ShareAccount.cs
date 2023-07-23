using MicroFinance.Models.ClientSetup;
namespace MicroFinance.Models.Share
{
    public class ShareAccount
    {
        public int Id { get; set; }
        public int AccountNumber { get; set; }
        public virtual Client Client { get; set; }
        public decimal CurrentShareBalance { get; set; }
        public int CurrentNumberOfKitta { get; set; }
        public DateTime? StartOn { get; set; }
        public DateTime? EndOn { get; set; }
        public bool IsActive { get; set; }
    }
}