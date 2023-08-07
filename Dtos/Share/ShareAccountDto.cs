using MicroFinance.Enums.Client;
namespace MicroFinance.Dtos.Share
{
    public class ShareAccountDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public decimal CurrentShareBalance { get; set; }
        public bool IsActive { get; set; }
        public string ClientName { get; set; }
        public ShareTypeEnum ShareType { get; set; }
    }
}