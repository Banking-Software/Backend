using MicroFinance.Dtos.CompanyProfile;
using MicroFinance.Models.CompanyProfile;

namespace MicroFinance.Repository.CompanyProfile
{
    public interface ICompanyProfileRepository
    {
        Task<int> CreateBranch(Branch branch);
        Task<int> UpdateBranch(UpdateBranchDto branchDto, string modifiedBy);
        Task<Branch> GetBranchById(int id);
        Task<Branch> GetBranchByBranchCode(string branchCode);
        Task<List<Branch>> GetBranches();
    }
}