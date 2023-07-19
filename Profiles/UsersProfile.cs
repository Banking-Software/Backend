using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.AccountSetup.MainLedger;
using MicroFinance.Dtos.UserManagement;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.UserManagement;

namespace MicroFinance.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            // START: SuperAdmin Mapping Part
            CreateMap<SuperAdmin, SuperAdminDto>();
            CreateMap<SuperAdminLoginDto, SuperAdmin>();
            // END
            
            // START: ADMIN CREATION BY SUPERADMIN Mapping
            CreateMap<CreateAdminBySuperAdminDto, CreateEmployeeDto>();
            CreateMap<CreateAdminBySuperAdminDto, UserRegisterDto>();
            CreateMap<UserDetailsDto, UserDetailsToSuperAdmin>()
            .ForMember(dest=>dest.UserId, opt=>opt.MapFrom(src=>src.UserData.UserId))
            .ForMember(dest=>dest.UserName, opt=>opt.MapFrom(src=>src.UserData.UserName))
            .ForMember(dest=>dest.IsActive, opt=>opt.MapFrom(src=>src.UserData.IsActive))
            .ForMember(dest=>dest.Role, opt=>opt.MapFrom(src=>src.UserData.Role));
            // END

            // START: Create Employee by Admin Mapping
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserRegisterDto, Employee>();
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>();
            CreateMap<User, UserDto>()
            .ForMember(dest=>dest.UserId, opt=> opt.MapFrom(src=>src.Id));
            CreateMap<Employee, EmployeeDto>()
            .ForMember(dest=>dest.Gender, opt=>opt.MapFrom(src=>src.GenderCode==1?"पुरूष":(src.GenderCode==2?"महिला":null)))
            .ForMember(dest=>dest.CitizenShipFileData, opt=>opt.MapFrom(src => (src.CitizenShipFileData != null ? Convert.ToBase64String(src.CitizenShipFileData) : null)))
            .ForMember(dest=>dest.ProfilePicFileData, opt=>opt.MapFrom(src => (src.ProfilePicFileData != null ? Convert.ToBase64String(src.ProfilePicFileData) : null)))
            .ForMember(dest=>dest.SignatureFileData, opt=>opt.MapFrom(src => (src.SignatureFileData != null ? Convert.ToBase64String(src.SignatureFileData) : null)));

            // END

            // TokenDto
            CreateMap<UserDto, TokenDto>()
            .ForMember(dest=>dest.IsActive, opt=>opt.MapFrom(src=>src.IsActive.ToString()));
        }
    }
}