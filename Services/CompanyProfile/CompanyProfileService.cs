using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.CompanyProfile;
using MicroFinance.Enums;
using MicroFinance.Models.CompanyProfile;
using MicroFinance.Repository.CompanyProfile;

namespace MicroFinance.Services.CompanyProfile
{
    public class CompanyProfileService : ICompanyProfileService
    {
        private readonly ICompanyProfileRepository _companyProfile;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public CompanyProfileService(ICompanyProfileRepository companyProfile, IMapper mapper, IConfiguration config)
        {
            _companyProfile = companyProfile;
            _mapper = mapper;
            _config=config;
        }


        private Task<CompanyDetail> UploadImage(CompanyDetail companyDetail, IFormFile? companyLogo)
        {
            if(companyLogo==null)
                return Task.FromResult(companyDetail);
            string fileExtenstion = (Path.GetExtension(companyLogo.FileName)).Replace(".", "").ToUpper();
            try
            {
                float maxFileValue = float.Parse(_config["ApplicationSettings:ImageMaxSize"]);
                double maxFileSize = maxFileValue*1024*1024; // 3MB 
                if(companyLogo.Length>maxFileSize) 
                    throw new Exception($"File size exceeded the Limit. Upto {maxFileSize}MB is allowed while {companyLogo.Length}MB is received");
                var fileType = (FileType)Enum.Parse(typeof(FileType), fileExtenstion);
                using (var stream = new MemoryStream())
                {
                
                    companyLogo.CopyTo(stream);
                    companyDetail.LogoFileData = stream.ToArray();
                    companyDetail.LogoFileName = companyLogo.FileName;
                    companyDetail.LogoFileType = fileType;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Given file is not allowed. {ex.Message}");
            }
            return Task.FromResult(companyDetail);
        }
        public async Task<ResponseDto> CreateCompanyProfileService(CreateCompanyProfileDto createCompanyProfileDto)
        {
            var companyProfile = await _companyProfile.GetCompanyDetail();
            if(companyProfile!=null) 
                throw new Exception("Company Profile already exist. Not allowed to create again");

            CompanyDetail newCompanyDetail = _mapper.Map<CompanyDetail>(createCompanyProfileDto);
            newCompanyDetail = await UploadImage(newCompanyDetail, createCompanyProfileDto?.CompanyLogo);
            var creationStatus = await _companyProfile.CreateCompanyProfile(newCompanyDetail);
            if (creationStatus >= 1)
            {
                return new ResponseDto() { Message = "Comapny Profile Created Successfully", Status = true, StatusCode = "200" };
            }
            throw new Exception("Failed to create the company profile");
        }

        public async Task<ResponseDto> UpdateCompanyProfileService(UpdateCompanyProfileDto updateCompanyProfileDto)
        {
            CompanyDetail existingCompanyDetail = await _companyProfile.GetCompanyDetail();
            if(existingCompanyDetail==null) 
                throw new NotImplementedException("No details found");
            CompanyDetail updateCompanydetail = _mapper.Map<CompanyDetail>(updateCompanyProfileDto);
            updateCompanydetail.Id=existingCompanyDetail.Id;
            if(updateCompanyProfileDto.IsLogoChanged)
            {
                updateCompanydetail = await UploadImage(updateCompanydetail, updateCompanyProfileDto?.CompanyLogo);
            }
            else
            {
                updateCompanydetail.LogoFileData = existingCompanyDetail.LogoFileData;
                updateCompanydetail.LogoFileName = existingCompanyDetail.LogoFileName;
                updateCompanydetail.LogoFileType = existingCompanyDetail.LogoFileType;
            }
            var updateCompanyDetail = await _companyProfile.UpdateCompanyProfile(updateCompanydetail);
            if (updateCompanyDetail >= 1)
            {
                return new ResponseDto() { Message = "Update Successfull", Status = true, StatusCode = "200" };
            }
            throw new Exception("Failed to Update the coompany profile");
        }

        public async Task<CompanyProfileDto> GetCompanyProfileService()
        {
            var companyDetail = await _companyProfile.GetCompanyDetail();
            if(companyDetail==null) 
                throw new Exception("Company Profile Has not Created Yet");
            var companyDetailDto = _mapper.Map<CompanyProfileDto>(companyDetail);
            if(companyDetail.LogoFileData!=null)
            {
                companyDetailDto.LogoFileData = Convert.ToBase64String(companyDetail.LogoFileData);
            }
            return companyDetailDto;
        }

        // Branch Section
        public async Task<ResponseDto> CreateBranchService(CreateBranchDto createBranchDto, string createdBy)
        {
            var branchExist = await _companyProfile.GetBranchByBranchCode(createBranchDto.BranchCode);
            if (branchExist != null) throw new NotSupportedException("Branch Already Exist");
            Branch branch = new Branch();
            branch.BranchCode = createBranchDto.BranchCode;
            branch.BranchName = createBranchDto.BranchName;
            branch.IsActive = createBranchDto.IsActive;
            branch.CreatedBy = createdBy;
            branch.CreatedOn = DateTime.Now;
            int branchId = await _companyProfile.CreateBranch(branch);
            if (branchId >= 1) return new ResponseDto() { Message = "Branch Created Successfully", Status = true, StatusCode = "202" };
            throw new Exception("Not able to Create a Branch");
        }


        public async Task<List<BranchDto>> GetAllBranchsService()
        {
            var branches = await _companyProfile.GetBranches();
            if (branches != null && branches.Count >= 1)
                return _mapper.Map<List<BranchDto>>(branches);
            else if (branches != null && branches.Count == 0)
                return new List<BranchDto>();
            throw new BadHttpRequestException("Bad Request");
        }

        public async Task<BranchDto> GetBranchServiceByBranchCodeService(string branchCode)
        {
            Branch branch = await _companyProfile.GetBranchByBranchCode(branchCode);
            if (branch == null) throw new NotImplementedException("Invalide Branch Code");
            return _mapper.Map<BranchDto>(branch);
        }

        public async Task<BranchDto> GetBranchServiceById(int id)
        {
            Branch branch = await _companyProfile.GetBranchById(id);
            if (branch == null) throw new NotImplementedException("Invalide Branch Id");
            return _mapper.Map<BranchDto>(branch);
        }

        public async Task<ResponseDto> UpdateBranchService(UpdateBranchDto updateBranchDto, string modifiedBy)
        {
            Branch branchExist = await _companyProfile.GetBranchById(updateBranchDto.Id);
            if (branchExist != null)
            {
                int branchId = await _companyProfile.UpdateBranch(updateBranchDto, modifiedBy);
                if (branchId >= 1) return new ResponseDto() { Message = "Update Successfull", Status = true, StatusCode = "202" };
                throw new Exception("Unable to Update the Branch");
            }
            throw new NotImplementedException("No Branch Found");
        }
       
        public async Task<ResponseDto> CreateCalenderService(List<CreateCalenderDto> createCalenderDtos, Dictionary<string, string> userClaim)
        {
            CalendarServiceDto validateCalender = new CalendarServiceDto();
            validateCalender.ValidateCalenderList(createCalenderDtos);
            List<Calendar> calendars = new List<Calendar>();
            foreach (var calenderDto in createCalenderDtos)
            {
                var calendar = _mapper.Map<Calendar>(calenderDto);
                calendar.CreatedBy = userClaim["currentUserName"];
                calendar.CreatedOn = DateTime.Now;
                calendars.Add(calendar);
            }

            int createStatus = await _companyProfile.CreateCalender(calendars);
            if (createStatus >= 1)
            {
                return new ResponseDto() { Message = "Calender Created Successfully", Status = true, StatusCode = "200" };
            }
            throw new Exception("Not able to create a calender");
        }

        public async Task<ResponseDto> UpdateCalenderService(UpdateCalenderDto updateCalenderDto, Dictionary<string, string> userClaim)
        {
            var currentCurrrentCalender = await _companyProfile.GetCalendarById(updateCalenderDto.Id);
            if (currentCurrrentCalender == null) throw new Exception("No Calender Found");
            if (currentCurrrentCalender.IsActive == true || currentCurrrentCalender.IsLocked == true)
            {
                throw new Exception("Calender is locked, not allowed to edit. Please Contact Software Provider");
            }
            int updateStatus = await _companyProfile.UpdateCalender(updateCalenderDto, userClaim);
            if (updateStatus >= 1)
            {
                return new ResponseDto() { Message = "Update Successfully", Status = true, StatusCode = "200" };
            }
            throw new Exception("Unable to update the calender");
        }

        public async Task<List<CalendarDto>> GetAllCalenderService()
        {
            var calendars = await _companyProfile.GetAllCalendars();
            return _mapper.Map<List<CalendarDto>>(calendars);
        }

        public async Task<CalendarDto> GetCalendarByIdService(int id)
        {
            var calendar = await _companyProfile.GetCalendarById(id);
            if (calendar != null)
            {
                return _mapper.Map<CalendarDto>(calendar);
            }
            throw new Exception("No such Calender Exist");
        }

        public async Task<List<CalendarDto>> GetCalendarByYearService(int year)
        {
            var calendarByYear = await _companyProfile.GetCalendarByYear(year);
            if (calendarByYear != null)
            {
                return _mapper.Map<List<CalendarDto>>(calendarByYear);
            }
            throw new Exception("No calender exist for given year");
        }

        public async Task<CalendarDto> GetCurrentActiveCalenderService()
        {
            var activeCalender = await _companyProfile.GetActiveCalender();
            if (activeCalender != null)
            {
                return _mapper.Map<CalendarDto>(activeCalender);
            }
            throw new Exception("No Active Calender Exist");
        }

         public async Task<CalendarDto> GetCalendarByYearAndMonthService(int year, int month)
         {
            var calendarByYearAndMonth = await _companyProfile.GetCalendarByYearAndMonth(year, month);
            if(calendarByYearAndMonth!=null)
            {
                return _mapper.Map<CalendarDto>(calendarByYearAndMonth);
            }
            throw new Exception($"Unable Get Calendar for year {year} and month {month}");
         }
    }
}