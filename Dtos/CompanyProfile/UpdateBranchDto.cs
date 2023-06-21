namespace MicroFinance.Dtos.CompanyProfile
{
    public class UpdateBranchDto
    {
        public int Id { get; set; }
        public string? BranchName { get; set; }
        public bool IsActive { get; set; }
    }
}