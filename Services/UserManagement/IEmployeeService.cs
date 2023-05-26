using MicroFinance.Dtos;
using MicroFinance.Dtos.UserManagement;
using MicroFinance.Models.UserManagement;
using Microsoft.AspNetCore.Identity;

namespace MicroFinance.Services.UserManagement
{
    public interface IEmployeeService
    {
        // START: Service for Authorized User
        Task<ResponseDto> RegisterService(UserRegisterDto userRegisterDto);
        Task<TokenResponseDto> LoginService(UserLoginDto userLoginDto);
        Task<ResponseDto> UpdatePasswordService(UpdateUserPasswordDto updateUserPasswordDto, string userName);
        Task<ResponseDto> UpdateUserProfileService(UserProfileUpdateDto userProfileUpdateDto);
        Task<UserDto> GetUserByUserNameService(string userName);
        Task<UserDetailsDto> GetUserDetailsByUserNameService(string userName);
        Task<UserDto> GetUserByIdService(string id);
        Task<string> GetRole(string id);
        Task<ResponseDto> DeleteUser(string userName);
        Task<UserDetailsDto> GetUserDetailsByIdService(string userName);
        Task<List<UserDetailsDto>> GetUsersDetailsService();
        Task<ResponseDto> AssignRoleService(string userName, string role);

        // END

        // START: Service for Employee

        Task<ResponseDto> CreateEmployeeService(CreateEmployeeDto createEmployeeDto);
        Task<ResponseDto> EditProfileService(CreateEmployeeDto createEmployeeDto);
        Task<EmployeeDto> GetEmployeeByUserName(string userName);
        Task<EmployeeDto> GetEmployeeById(int id);
        Task<List<EmployeeDto>> GetEmployess();
        Task<ResponseDto> DeleteEmployee(string userName);
        // END
    }
}