using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using MicroFinance.DBContext.UserManagement;
using MicroFinance.Dtos;
using MicroFinance.Dtos.UserManagement;
using MicroFinance.ErrorManage;
using MicroFinance.Exceptions;
using MicroFinance.Models.UserManagement;
using MicroFinance.Repository.UserManagement;
using MicroFinance.Role;
using MicroFinance.Services;
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

        public SuperAdminController
        (
            ILogger<SuperAdminController> logger,
            ISuperAdminService superAdminService,
            IMapper mapper
        )
        {
            _logger = logger;
            _mapper = mapper;
            _superAdminService = superAdminService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<TokenResponseDto>> Register(SuperAdminRegisterDto superAdminRegisterDto)
        {
            var superadmin = _mapper.Map<SuperAdmin>(superAdminRegisterDto);
            var result = await _superAdminService.RegisterService(superAdminRegisterDto);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(SuperAdminLoginDto superAdminLoginDto)
        {

            var superadmin = await _superAdminService.LoginService(superAdminLoginDto);
            return Ok(superadmin);

        }

        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpPut("update-password")]
        public async Task<ActionResult<ResponseDto>> UpdatePassword(SuperAdminUpdatePasswordDto superAdminUpdatePasswordDto)
        {

            string userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            var updatePassword = await _superAdminService.UpdatePasswordService(superAdminUpdatePasswordDto, userName);
            return Ok(updatePassword);

        }

        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpPost("create-admin")]
        public async Task<ActionResult<ResponseDto>> CreateAdmin(CreateAdminBySuperAdminDto createAdmin)
        {

            if (createAdmin.Role != UserRole.Officer)
                throw new UnAuthorizedExceptionHandler("You are only authorized to create 'Officer'");
            var userCreate = await _superAdminService.CreateAdminService(createAdmin);
            return Ok(userCreate);

        }
        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getusers")]
        public async Task<ActionResult<List<UserDetailsToSuperAdmin>>> GetUsers()
        {

            var users = await _superAdminService.GetMicroFinanceUserSerivce();
            if (users.Count <= 1 && users[0].Message != null)
            {
                throw new NotFoundExceptionHandler(users[0].Message);
            }
            return _mapper.Map<List<UserDetailsToSuperAdmin>>(users);

        }

        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpPut("activate-deactivate")]
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


        // [HttpGet("approve-user/{userName}")]
        // public async Task<ActionResult<AuthorizedUser>> ApproveUser(string userName)
        // {
        //     var approvableUserStatus = await _employeeRepo.ApproveUser(userName);
        //     if (approvableUserStatus <= 0) return BadRequest();
        //     var usersWithCredentialDetails = await _employeeRepo.GetAuthorizedUserDetailsByUsername(userName);
        //     return usersWithCredentialDetails;
        // }
    }
}