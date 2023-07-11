using MicroFinance.Dtos.CompanyProfile;
using MicroFinance.Models.CompanyProfile;

namespace MicroFinance.Repository.CompanyProfile
{
    public interface ICompanyProfileRepository
    {

        // Create Company Profile
        Task<int> CreateCompanyProfile(CompanyDetail companyDetail);
        Task<int> UpdateCompanyProfile(UpdateCompanyProfileDto updateCompanyProfileDto);
        Task<CompanyDetail> GetCompanyDetailById(int id);


        // Create Company Branch
        Task<int> CreateBranch(Branch branch);
        Task<int> UpdateBranch(UpdateBranchDto branchDto, string modifiedBy);
        Task<Branch> GetBranchById(int id);
        Task<Branch> GetBranchByBranchCode(string branchCode);
        Task<List<Branch>> GetBranches();
    }
}