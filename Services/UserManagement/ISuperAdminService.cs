using MicroFinance.Dtos;
using MicroFinance.Dtos.UserManagement;
using MicroFinance.Models.UserManagement;
using Microsoft.AspNetCore.Identity;

namespace MicroFinance.Services.UserManagement
{
    public interface ISuperAdminService
    {
        // Super Admin 
        Task<TokenResponseDto> RegisterService(SuperAdminRegisterDto superAdminRegisterDto);
        Task<TokenResponseDto> LoginService(SuperAdminLoginDto superAdminLoginDto);
        Task<SuperAdminDto> GetUserByIdService(string id);
        Task<ResponseDto> UpdatePasswordService(SuperAdminUpdatePasswordDto superAdminUpdatePasswordDto, string userName);

        // Handling Admin
        Task<ResponseDto> ActivateDeactivateMicroFinanceUserService(string userName, bool isActive);
        Task<ResponseDto> CreateAdminService(CreateAdminBySuperAdminDto createAdminBySuperAdminDto);
        Task<List<UserDetailsDto>> GetMicroFinanceUserSerivce();
        // END
    }
}