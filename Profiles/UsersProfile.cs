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
            CreateMap<User, UserDto>()
            .ForMember(dest=>dest.UserId, opt=> opt.MapFrom(src=>src.Id));
            CreateMap<Employee, EmployeeDto>();
            // END

            // TokenDto
            CreateMap<UserDto, TokenDto>()
            .ForMember(dest=>dest.IsActive, opt=>opt.MapFrom(src=>src.IsActive.ToString()));
        }
    }
}