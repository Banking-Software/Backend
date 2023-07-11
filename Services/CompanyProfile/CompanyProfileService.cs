using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.CompanyProfile;
using MicroFinance.Models.CompanyProfile;
using MicroFinance.Repository.CompanyProfile;

namespace MicroFinance.Services.CompanyProfile
{
    public class CompanyProfileService : ICompanyProfileService
    {
        private readonly ICompanyProfileRepository _companyProfile;
        private readonly IMapper _mapper;

        public CompanyProfileService(ICompanyProfileRepository companyProfile, IMapper mapper)
        {
            _companyProfile =companyProfile;
            _mapper=mapper;
        }
        public async Task<ResponseDto> CreateBranchService(CreateBranchDto createBranchDto, string createdBy)
        {
            var branchExist = await _companyProfile.GetBranchByBranchCode(createBranchDto.BranchCode);
            if(branchExist!=null) throw new NotSupportedException("Branch Already Exist");
            Branch branch = new Branch();
            branch.BranchCode=createBranchDto.BranchCode;
            branch.BranchName=createBranchDto.BranchName;
            branch.IsActive=createBranchDto.IsActive;
            branch.CreatedBy=createdBy;
            branch.CreatedOn=DateTime.Now;
            int branchId = await _companyProfile.CreateBranch(branch);
            if(branchId>=1) return new ResponseDto(){Message="Branch Created Successfully", Status=true, StatusCode="202"};
            throw new Exception("Not able to Create a Branch");
        }

        public async Task<ResponseDto> CreateCompanyProfileService(CreateCompanyProfileDto createCompanyProfileDto)
        {
            CompanyDetail newCompanyDetail = _mapper.Map<CompanyDetail>(createCompanyProfileDto);
            var creationStatus  = await _companyProfile.CreateCompanyProfile(newCompanyDetail);
            if(creationStatus>=1)
            {
                return new ResponseDto(){Message="Comapny Profile Created Successfully", Status=true, StatusCode="200"};
            }
            throw new Exception("Failed to create the company profile");
        }

        public async Task<List<BranchDto>> GetAllBranchsService()
        {
            var branches = await _companyProfile.GetBranches();
            if(branches!=null && branches.Count>=1)
                return _mapper.Map<List<BranchDto>>(branches);
            else if (branches!=null && branches.Count==0)
                return new List<BranchDto>();
            throw new BadHttpRequestException("Bad Request");
        }

        public async Task<BranchDto> GetBranchServiceByBranchCodeService(string branchCode)
        {
            Branch branch = await _companyProfile.GetBranchByBranchCode(branchCode);
            if(branch==null) throw new NotImplementedException("Invalide Branch Code");
            return _mapper.Map<BranchDto>(branch);
        }

        public async Task<BranchDto> GetBranchServiceById(int id)
        {
            Branch branch = await _companyProfile.GetBranchById(id);
            if(branch==null) throw new NotImplementedException("Invalide Branch Id");
            return _mapper.Map<BranchDto>(branch);
        }

        public async Task<CompanyProfileDto> GetCompanyProfileByIdService(int id)
        {
            var companyDetail=await _companyProfile.GetCompanyDetailById(id);
            return _mapper.Map<CompanyProfileDto>(companyDetail);
        }

        public async Task<ResponseDto> UpdateBranchService(UpdateBranchDto updateBranchDto, string modifiedBy)
        {
            Branch branchExist = await _companyProfile.GetBranchById(updateBranchDto.Id);
            if(branchExist!=null)
            {
                int branchId = await _companyProfile.UpdateBranch(updateBranchDto, modifiedBy);
                if(branchId>=1) return new ResponseDto(){Message="Update Successfull", Status=true, StatusCode="202"};
                throw new Exception("Unable to Update the Branch");
            }
            throw new NotImplementedException("No Branch Found");
        }

        public async Task<ResponseDto> UpdateCompanyProfileService(UpdateCompanyProfileDto updateCompanyProfileDto)
        {
            var updateCompanyDetail = await _companyProfile.UpdateCompanyProfile(updateCompanyProfileDto);
            if(updateCompanyDetail>=1)
            {
                return new ResponseDto(){Message="Update Successfull", Status=true, StatusCode="200"};
            }
            throw new Exception("Failed to Update the coompany profile");
        }
    }
}