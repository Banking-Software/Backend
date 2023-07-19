using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using MicroFinance.DBContext.UserManagement;
using MicroFinance.Dtos;
using MicroFinance.Dtos.CompanyProfile;
using MicroFinance.Dtos.UserManagement;
using MicroFinance.ErrorManage;
using MicroFinance.Exceptions;
using MicroFinance.Models.UserManagement;
using MicroFinance.Repository.UserManagement;
using MicroFinance.Role;
using MicroFinance.Services;
using MicroFinance.Services.CompanyProfile;
using MicroFinance.Services.UserManagement;
using MicroFinance.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Controllers.UserManagement
{
    public class SuperAdminController : SuperAdminBaseApiController
    {
        private readonly ILogger<SuperAdminController> _logger;
        private readonly IMapper _mapper;
        private readonly ISuperAdminService _superAdminService;
        private readonly ICompanyProfileService _companyProfile;
        private readonly ITokenService _tokenService;

        public SuperAdminController
        (
            ILogger<SuperAdminController> logger,
            ISuperAdminService superAdminService,
            IMapper mapper,
            ICompanyProfileService companyProfile,
            ITokenService tokenService
        )
        {
            _logger = logger;
            _mapper = mapper;
            _superAdminService = superAdminService;
            _companyProfile=companyProfile;
            _tokenService = tokenService;
        }

        private TokenDto GetDecodedToken()
        {
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var decodedToken = _tokenService.DecodeJWT(token);
            return decodedToken;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(SuperAdminLoginDto superAdminLoginDto)
        {

            var superadmin = await _superAdminService.LoginService(superAdminLoginDto);
            return Ok(superadmin);

        }

        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpPut("updatePassword")]
        public async Task<ActionResult<ResponseDto>> UpdatePassword(SuperAdminUpdatePasswordDto superAdminUpdatePasswordDto)
        {

            string userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            var updatePassword = await _superAdminService.UpdatePasswordService(superAdminUpdatePasswordDto, userName);
            return Ok(updatePassword);

        }
        
        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpPost("createAdmin")]
        public async Task<ActionResult<ResponseDto>> CreateAdmin(CreateAdminBySuperAdminDto createAdmin)
        {
            string currentUser = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            if (createAdmin.Role != UserRole.Officer)
                throw new UnAuthorizedExceptionHandler("You are only authorized to create 'Officer'");
            var userCreate = await _superAdminService.CreateAdminService(createAdmin, currentUser);
            return Ok(userCreate);

        }
        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getAllUsers")]
        public async Task<ActionResult<List<UserDetailsToSuperAdmin>>> GetAllUsers()
        {

            var users = await _superAdminService.GetMicroFinanceUserSerivce();
            if (users.Count <= 1 && users[0].Message != null)
            {
                throw new NotFoundExceptionHandler(users[0].Message);
            }
            return _mapper.Map<List<UserDetailsToSuperAdmin>>(users);

        }

        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpPut("activateDeactivateUser")]
        public async Task<ActionResult<List<ActiveDeactiveInformation>>> ActivateDeactivateUsers(List<ActivateDeactivateUserDto> activateDeactivateUserDto)
        {

                // var usersInfo = activateDeactivateUserDto.UserStatus;
                var updatedUserInfo = new List<ActiveDeactiveInformation>();
                foreach (var user in activateDeactivateUserDto)
                {
                    var updateStatus =
                    await _superAdminService.ActivateDeactivateMicroFinanceUserService(user.UserName, user.IsActive);
                    var responseInformation = new ActiveDeactiveInformation();
                    responseInformation.UserName = user.UserName;
                    responseInformation.IsActive = user.IsActive;
                    responseInformation.Message = updateStatus.Message;
                    responseInformation.Status = updateStatus.Status;
                    updatedUserInfo.Add(responseInformation);

                }
                return Ok(updatedUserInfo);
        }
    }
}