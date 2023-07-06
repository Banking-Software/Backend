using MicroFinance.Dtos;
using MicroFinance.Dtos.UserManagement;
using MicroFinance.Models.UserManagement;
using Microsoft.AspNetCore.Identity;

namespace MicroFinance.Services.UserManagement
{
    public interface IEmployeeService
    {
        // START: Service for Authorized User
        Task<ResponseDto> RegisterService(UserRegisterDto userRegisterDto, string createdBy);
        Task<TokenResponseDto> LoginService(UserLoginDto userLoginDto);
        Task<ResponseDto> UpdatePasswordService(UpdateUserPasswordDto updateUserPasswordDto, string userName);
        Task<ResponseDto> UpdateUserProfileService(UserProfileUpdateDto userProfileUpdateDto, string modifiedBy);
        Task<UserDto> GetUserByUserNameService(string userName);
        Task<UserDto> GetUserByEmailService(string email);
        Task<UserDetailsDto> GetUserDetailsByUserNameService(string userName);
        Task<UserDetailsDto> GetUserDetailsByEmailService(string email);
        Task<UserDto> GetUserByIdService(string id);
        Task<string> GetRole(string id);
        Task<UserDetailsDto> GetUserDetailsByIdService(string userName);
        Task<List<UserDetailsDto>> GetUsersDetailsService();
        Task<ResponseDto> AssignRoleService(string userName, string role);

        // END

        // START: Service for Employee

        Task<ResponseDto> CreateEmployeeService(CreateEmployeeDto createEmployeeDto, string createdBy,string companyName);
        Task<ResponseDto> EditProfileService(UpdateEmployeeDto updateEmployeeDto, Dictionary<string, string> claimsParameters);
        Task<EmployeeDto> GetEmployeeByEmail(string email);
        Task<EmployeeDto> GetEmployeeById(int id);
        Task<List<EmployeeDto>> GetEmployess();
        // END
    }
}