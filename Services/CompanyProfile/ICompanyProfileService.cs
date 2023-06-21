using MicroFinance.Dtos;
using MicroFinance.Dtos.CompanyProfile;

namespace MicroFinance.Services.CompanyProfile
{
    public interface ICompanyProfileService
    {
        Task<ResponseDto> CreateBranchService(CreateBranchDto createBranchDto, string createdBy);
        Task<ResponseDto> UpdateBranchService(UpdateBranchDto updateBranchDto, string modifiedBy);
        Task<BranchDto> GetBranchServiceById(int id);
        Task<BranchDto> GetBranchServiceByBranchCodeService(string branchCode);
        Task<List<BranchDto>> GetAllBranchsService();
    }
}