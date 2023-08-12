using MicroFinance.DBContext;
using MicroFinance.Dtos.CompanyProfile;
using MicroFinance.Models.CompanyProfile;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.CompanyProfile
{
    public class CompanyProfileRepository : ICompanyProfileRepository
    {
        private readonly ApplicationDbContext _companyProfileDbContex;

        public CompanyProfileRepository(ApplicationDbContext companyProfileDbContex)
        {
            _companyProfileDbContex=companyProfileDbContex;
        }
        public async Task<int> CreateBranch(Branch branch)
        {
            await _companyProfileDbContex.Branches.AddAsync(branch);
            return await _companyProfileDbContex.SaveChangesAsync();
        }
        public async Task<int> CreateCompanyProfile(CompanyDetail companyDetail)
        {
            await _companyProfileDbContex.CompanyDetails.AddAsync(companyDetail);
            await _companyProfileDbContex.SaveChangesAsync();
            return companyDetail.Id;
        }
        public async Task<Branch> GetBranchByBranchCode(string branchCode)
        {
            var branch = await _companyProfileDbContex.Branches.Where(b=>b.BranchCode==branchCode).FirstOrDefaultAsync();
            return branch;
        }

        public async Task<Branch> GetBranchById(int id)
        {
            var branch = await _companyProfileDbContex.Branches.Where(b=>b.Id==id).FirstOrDefaultAsync();
            return branch;
        }

        public async Task<List<Branch>> GetBranches()
        {
            return await _companyProfileDbContex.Branches.ToListAsync();
        }
        public async Task<CompanyDetail> GetCompanyDetail()
        {
            return await _companyProfileDbContex.CompanyDetails.FirstOrDefaultAsync();
        }

        public async Task<int> UpdateBranch(UpdateBranchDto branchDto, string modifiedBy)
        {
            var existingBranch = await _companyProfileDbContex.Branches.Where(b=>b.Id==branchDto.Id).FirstOrDefaultAsync();
            existingBranch.BranchName=branchDto.BranchName;
            existingBranch.IsActive=branchDto.IsActive;
            existingBranch.ModifiedOn = DateTime.Now;
            existingBranch.ModifiedBy=modifiedBy;
            return await _companyProfileDbContex.SaveChangesAsync();
        }

        public async Task<int> UpdateCompanyProfile(CompanyDetail updateCompanyDetail)
        {
            var exisitngProfile = await _companyProfileDbContex.CompanyDetails.FindAsync(updateCompanyDetail.Id);
            _companyProfileDbContex.Entry(exisitngProfile).State = EntityState.Detached;

           _companyProfileDbContex.CompanyDetails.Attach(updateCompanyDetail);
           _companyProfileDbContex.Entry(updateCompanyDetail).State = EntityState.Modified;
           return await _companyProfileDbContex.SaveChangesAsync();  
        }

        // Calender//

        public async Task<int> CreateCalender(List<Calendar> calendars)
        {
            await _companyProfileDbContex.Calendars.AddRangeAsync(calendars);
            return await _companyProfileDbContex.SaveChangesAsync();
        }
        public async Task<int> UpdateCalender(UpdateCalenderDto updateCalender, Dictionary<string, string> userClaim)
        {
            var existingCalender =  await _companyProfileDbContex.Calendars.FindAsync(updateCalender.Id);
            existingCalender.Year = updateCalender.Year;
            existingCalender.Month = updateCalender.Month;
            existingCalender.MonthName = updateCalender.MonthName;
            existingCalender.NumberOfDay = updateCalender.NumberOfDay;
            existingCalender.RunningDay = updateCalender.RunningDay!=null?(int) updateCalender.RunningDay:existingCalender.RunningDay;
            if(existingCalender.IsActive)
            {
                var otherActiveCalender = await _companyProfileDbContex.Calendars.Where(c=>c.IsActive && c.Id!=existingCalender.Id).FirstOrDefaultAsync();
                if(otherActiveCalender!=null) 
                    throw new Exception($"Calender with id: {existingCalender.Id} and {otherActiveCalender.Id} both are active. So unable to update the month. Please contact software provider");
                existingCalender.IsLocked = true;
            } 
            existingCalender.ModifiedBy = userClaim["currentUserName"];
            existingCalender.ModifiedOn = DateTime.Now;
            return await _companyProfileDbContex.SaveChangesAsync();
        }

        public async Task<Calendar> GetCalendarById(int id)
        {
            return await _companyProfileDbContex.Calendars.FindAsync(id);
        }

        public async Task<Calendar> GetActiveCalender()
        {
            return await _companyProfileDbContex.Calendars.Where(c=>c.IsActive==true).FirstOrDefaultAsync();
        }

        public async Task<int> GetActiveYear()
        {
            return await _companyProfileDbContex.Calendars.Where(c=>c.IsActive).Select(c=>c.Year).FirstOrDefaultAsync();
        }

        public async Task<List<Calendar>> GetAllCalendars()
        {
            return await _companyProfileDbContex.Calendars.ToListAsync();
        }

        public async Task<List<Calendar>> GetCalendarByYear(int year)
        {
            return await _companyProfileDbContex.Calendars.Where(c=>c.Year==year).ToListAsync();
        }
        public async Task<Calendar> GetCalendarByYearAndMonth(int year, int month)
        {
            return await _companyProfileDbContex.Calendars.Where(c=>c.Year==year && c.Month==month).SingleOrDefaultAsync();
        }
    }
}