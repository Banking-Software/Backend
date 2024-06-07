namespace MicroFinance.Dtos.Share
{
    public class ShareKittaDto
    {
        public int Id { get; set; }
        public int PriceOfOneKitta { get; set; }
        public decimal CurrentKitta { get; set; }
        public bool IsActive { get; set; }
    }
}