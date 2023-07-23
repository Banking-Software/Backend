namespace MicroFinance.Dtos.DepositSetup
{
    public class BaseDepositDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatorId { get; set; }
        public string BranchCode { get; set; }
        public DateTime RealWorldCreationDate { get; set; }
        public DateTime CompanyCalendarCreationDate { get; set; }

        public string? ModifiedBy { get; set; }
        public string? ModifierId { get; set; }
        public string? ModifierBranchCode { get; set; }
        public DateTime? RealWorldModificationDate { get; set; }
        public DateTime? CompanyCalendarModificationDate { get; set; }
    }
}