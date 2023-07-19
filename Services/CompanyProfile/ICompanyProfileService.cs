using MicroFinance.Dtos;
using MicroFinance.Dtos.CompanyProfile;

namespace MicroFinance.Services.CompanyProfile
{
    public interface ICompanyProfileService
    {
        Task<ResponseDto> CreateCompanyProfileService(CreateCompanyProfileDto createCompanyProfileDto);
        Task<ResponseDto> UpdateCompanyProfileService(UpdateCompanyProfileDto updateCompanyProfileDto);
        Task<CompanyProfileDto> GetCompanyProfileService();

        Task<ResponseDto> CreateBranchService(CreateBranchDto createBranchDto, string createdBy);
        Task<ResponseDto> UpdateBranchService(UpdateBranchDto updateBranchDto, string modifiedBy);
        Task<BranchDto> GetBranchServiceById(int id);
        Task<BranchDto> GetBranchServiceByBranchCodeService(string branchCode);
        Task<List<BranchDto>> GetAllBranchsService();

        Task<ResponseDto> CreateCalenderService(List<CreateCalenderDto> createCalenderDtos, Dictionary<string, string> userClaim);
        Task<ResponseDto> UpdateCalenderService(UpdateCalenderDto updateCalenderDto, Dictionary<string, string> userClaim);
        Task<CalendarDto> GetCalendarByIdService(int id);
        Task<CalendarDto> GetCurrentActiveCalenderService();
        Task<List<CalendarDto>> GetCalendarByYearService(int year);
        Task<List<CalendarDto>> GetAllCalenderService();
    }
}