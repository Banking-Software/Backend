namespace MicroFinance.Dtos.CompanyProfile
{
    public class CreateBranchDto
    {
        public string BranchCode { get; set; }
        public string? BranchName { get; set; }
        public bool IsActive { get; set; }
    }
}