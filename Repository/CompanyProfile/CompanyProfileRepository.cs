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

        public async Task<CompanyDetail> GetCompanyDetailById(int id)
        {
            return await _companyProfileDbContex.CompanyDetails.FindAsync(id);
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

        public async Task<int> UpdateCompanyProfile(UpdateCompanyProfileDto updateCompanyProfileDto)
        {
           CompanyDetail existingCompanyDetail = await _companyProfileDbContex.CompanyDetails.FindAsync(updateCompanyProfileDto.Id);
           if(existingCompanyDetail==null) throw new NotImplementedException("No details found");
           existingCompanyDetail.CompanyName = updateCompanyProfileDto.CompanyName;
           existingCompanyDetail.CompanyNameNepali  = updateCompanyProfileDto.CompanyNameNepali;
           existingCompanyDetail.CompanyAddress = updateCompanyProfileDto.CompanyAddress;
           existingCompanyDetail.CompanyAddressNepali = updateCompanyProfileDto.CompanyAddressNepali;
           existingCompanyDetail.CompanyEmailAddress = updateCompanyProfileDto.CompanyEmailAddress;
           existingCompanyDetail.EstablishedDate = updateCompanyProfileDto.EstablishedDate;
           existingCompanyDetail.FromDate=updateCompanyProfileDto.FromDate;
           existingCompanyDetail.PANNo=updateCompanyProfileDto.PANNo;
           existingCompanyDetail.PhoneNo=updateCompanyProfileDto.PhoneNo;
           return await _companyProfileDbContex.SaveChangesAsync();  
        }
    }
}