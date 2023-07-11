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
            CreateMap<CreateCompanyProfileDto, CompanyDetail>();
            CreateMap<CompanyDetail, CompanyProfileDto>(); 
        }
    }
}