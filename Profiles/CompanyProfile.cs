using AutoMapper;
using MicroFinance.Dtos.CompanyProfile;
using MicroFinance.Models.CompanyProfile;

namespace MicroFinance.Profiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Branch, BranchDto>();
            CreateMap<CreateCompanyProfileDto, CompanyDetail>()
            .ForMember(dest=>dest.LogoFileData, opt=>opt.Ignore());

            CreateMap<UpdateCompanyProfileDto, CompanyDetail>()
            .ForMember(dest=>dest.LogoFileData, opt=>opt.Ignore());
            
            CreateMap<CompanyDetail, CompanyProfileDto>(); 

            CreateMap<CreateCalenderDto, Calendar>()
            .ForMember(dest=>dest.IsLocked, opt=>opt.MapFrom(src=>src.IsActive==true?true:false))
            .ForMember(dest=>dest.RunningDay, opt=>opt.MapFrom(src=>(src.RunningDay!=null&&src.RunningDay>=1)?src.RunningDay:1));

            CreateMap<Calendar, CalendarDto>();
        }
    }
}