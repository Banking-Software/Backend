using MicroFinance.Dtos.CompanyProfile;
using MicroFinance.Models.CompanyProfile;

namespace MicroFinance.Repository.CompanyProfile
{
    public interface ICompanyProfileRepository
    {

        // Create Company Profile
        Task<int> CreateCompanyProfile(CompanyDetail companyDetail);
        Task<int> UpdateCompanyProfile(CompanyDetail updateCompanyDetail);
        Task<CompanyDetail> GetCompanyDetail();

        // Create Company Branch
        Task<int> CreateBranch(Branch branch);
        Task<int> UpdateBranch(UpdateBranchDto branchDto, string modifiedBy);
        Task<Branch> GetBranchById(int id);
        Task<Branch> GetBranchByBranchCode(string branchCode);
        Task<List<Branch>> GetBranches();

        // Create Calender
        Task<int> CreateCalender(List<Calendar> calendars);
        Task<int> UpdateCalender(UpdateCalenderDto updateCalender, Dictionary<string, string> userClaim);
        Task<Calendar> GetCalendarById(int id);
        Task<Calendar> GetCalendarByYearAndMonth(int year, int month);
        Task<int> GetActiveYear();
        Task<Calendar> GetActiveCalender();
        Task<List<Calendar>> GetAllCalendars();
        Task<List<Calendar>> GetCalendarByYear(int year);
    }
}