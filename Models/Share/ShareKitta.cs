namespace MicroFinance.Models.Share
{
    public class ShareKitta
    {
        public int Id { get; set; }
        public int PriceOfOneKitta { get; set; }
        public decimal CurrentKitta { get; set; }=0;
        public bool IsActive { get; set; }=true;
    }
}