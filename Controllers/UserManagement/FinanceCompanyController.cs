using System.Reflection;
using System.Security.Claims;
using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.UserManagement;
using MicroFinance.Enums;
using MicroFinance.ErrorManage;
using MicroFinance.Exceptions;
using MicroFinance.Role;
using MicroFinance.Services;
using MicroFinance.Services.CompanyProfile;
using MicroFinance.Services.UserManagement;
using MicroFinance.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroFinance.Controllers.UserManagement
{

    public class FinanceCompanyController : FinanceCompanyBaseApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<FinanceCompanyController> _logger;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ICompanyProfileService _companyProfileService;

        public FinanceCompanyController
        (
            IEmployeeService employeeService,
            ILogger<FinanceCompanyController> logger,
            IMapper mapper,
            ITokenService tokenService,
            ICompanyProfileService companyProfileService
        )
        {
            _employeeService = employeeService;
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
            _companyProfileService=companyProfileService;
        }

        private TokenDto GetDecodedToken()
        {
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var decodedToken = _tokenService.DecodeJWT(token);
            return decodedToken;
        }
        // START: API for Authorized user //
        [TypeFilter(typeof(IsUserOfficerFilter))]
        [HttpPost("createLoginCredential")]
        public async Task<ActionResult<ResponseDto>> CreateLoginCredential(UserRegisterDto userRegisterDto)
        {
            if (userRegisterDto.IsActive || userRegisterDto.Role==RoleEnum.Officer || userRegisterDto.Role==RoleEnum.SuperAdmin)
                throw new UnAuthorizedExceptionHandler("You are authorized to create user with provided information");
            var decodedToken = GetDecodedToken();
            return Ok(await _employeeService.RegisterService(userRegisterDto, decodedToken.UserName));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenResponseDto>> Login(UserLoginDto userLoginDto)
        {
            var user = await _employeeService.GetUserByUserNameService(userLoginDto.UserName);
            var companyProfile = await _companyProfileService.GetCompanyProfileService();
            if (user.IsActive == false)
                throw new UnAuthorizedExceptionHandler("Unauthorized Access. Contact Officer");
            if(companyProfile.CompanyValidityEndDate < DateTime.Now)
                throw new UnAuthorizedExceptionHandler($"Software Validity Ended on {companyProfile.CompanyValidityEndDate}. Please Contact Software Provider");
            var tokenResponseDto = await _employeeService.LoginService(userLoginDto);
            return Ok(tokenResponseDto);
        }

        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpPut("updateUserPassword")]
        public async Task<ActionResult<ResponseDto>> UpdateUserPassword(UpdateUserPasswordDto updateUserPasswordDto)
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            var updateStatus = await _employeeService.UpdatePasswordService(updateUserPasswordDto, userName);
            return Ok(updateStatus);
        }

        [TypeFilter(typeof(IsUserOfficerFilter))]
        [HttpPut("updateUserProfile")]
        public async Task<ActionResult<ResponseDto>> UpdateUserProfile(UserProfileUpdateDto userProfileUpdateDto)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _employeeService.UpdateUserProfileService(userProfileUpdateDto, decodedToken.UserName));
        }


        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getuser/username")]
        public async Task<ActionResult<UserDetailsDto>> GetUserByUserName([FromQuery] string userName)
        {

            var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var currentUserName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;

            //if (role != RoleEnum.Officer.ToString() && currentUserName != userName)
            if (role != RoleEnum.Officer.ToString() && currentUserName != userName)
                throw new UnAuthorizedExceptionHandler("Not Authorized to view");
            var user = await _employeeService.GetUserDetailsByUserNameService(userName);
            if (user.Message.ToString() != "Success")
                throw new NotFoundExceptionHandler(user.Message);
            return Ok(user);
        }


        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getUserById")]
        public async Task<ActionResult<UserDetailsDto>> GetUserById([FromQuery] string id)
        {

            var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (role != RoleEnum.Officer.ToString() && currentUserId != id)
                throw new UnAuthorizedExceptionHandler("Not Authorized to view");
            var user = await _employeeService.GetUserDetailsByIdService(id);
            if (user.Message.ToString() != "Success")
                throw new NotFoundExceptionHandler(user.Message);
            return Ok(user);

        }


        [TypeFilter(typeof(IsUserOfficerFilter))]
        [HttpGet("getAllUsers")]
        public async Task<ActionResult<List<UserDetailsDto>>> GetAllUsers()
        {

            var users = await _employeeService.GetUsersDetailsService();
            if (users.Count <= 1 && users[0].Message != null)
            {
                throw new NotFoundExceptionHandler(users[0].Message);
            }
            return Ok(users);

        }

        // [TypeFilter(typeof(IsUserOfficerFilter))]
        // [HttpGet("deleteuser")]
        // public async Task<ActionResult<ResponseDto>> DeleteUser([FromQuery] string username)
        // {
        //     try
        //     {
        //         var currentUserName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
        //         if (currentUserName == username)
        //             return Unauthorized(new ApiResponses(401, "You are authorized to remove yourself"));
        //         var response = await _employeeService.DeleteUser(username);
        //         return response;
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError($"{DateTime.Now} (FinanceCompany-DeleteUser) Exception: {ex.Message}");
        //         return BadRequest(new ApiResponses(400, "Bad Request"));
        //     }
        // }

        [TypeFilter(typeof(IsUserOfficerFilter))]
        [HttpPost("assignRoleToUser")]
        public async Task<ActionResult<ResponseDto>> AssignRole(AssignRoleDto assignRoleDto)
        {
            if(assignRoleDto.Role==RoleEnum.SuperAdmin)
                throw new UnauthorizedAccessException("You are not authorized to assign user to superadmin");
            var response = await _employeeService.AssignRoleService(assignRoleDto.UserName, assignRoleDto.Role);
            return response;
        }

        // END // 

        // START: API for company Employee //

        [TypeFilter(typeof(IsUserOfficerFilter))]
        [HttpPost("createEmployee")]
        public async Task<ActionResult<ResponseDto>> CreateEmployee([FromForm] CreateEmployeeDto createEmployeeDto)
        {
            var decodedToken = GetDecodedToken();
            var userCreate = await _employeeService
            .CreateEmployeeService(createEmployeeDto, decodedToken.UserName);
            return Ok(userCreate);
        }


        [TypeFilter(typeof(IsUserOfficerFilter))]
        [HttpPut("updateEmployeeProfile")]
        public async Task<ActionResult<ResponseDto>> UpdateEmployeeProfile([FromForm] UpdateEmployeeDto updateEmployeeDto)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _employeeService.EditProfileService(updateEmployeeDto, decodedToken));
        }

        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getemployeeByEmail")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeByEmail([FromQuery] string email)
        {
            var decodedToken = GetDecodedToken();

            if (decodedToken.Role != RoleEnum.Officer.ToString() && decodedToken.Email != email)
                throw new UnAuthorizedExceptionHandler("Not Authorized to view");

            var employee = await _employeeService.GetEmployeeByEmail(email);
            if (employee.Message != "Success")
                throw new NotFoundExceptionHandler(employee.Message);
            return Ok(employee);
        }



        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getemployeeById")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById([FromQuery] int id)
        {
            var decodedToken = GetDecodedToken();
            var role = decodedToken.Role;
            var currentUserId = decodedToken.UserId;
            var employee = await _employeeService.GetEmployeeById(id);
            if (employee.Message != "Success")
                throw new NotFoundExceptionHandler(employee.Message);

            if (role != RoleEnum.Officer.ToString())
            {
                var user = await _employeeService.GetUserByEmailService(employee.Email);
                if (currentUserId != user.UserId) throw new UnAuthorizedExceptionHandler("Not Authorized to view");
            }

            return Ok(employee);

        }
        [TypeFilter(typeof(IsUserOfficerFilter))]
        [HttpGet("getAllEmployees")]
        public async Task<ActionResult<List<EmployeeDto>>> GetEmployees()
        {

            var employees = await _employeeService.GetEmployess();
            if (employees.Count <= 1 && employees[0].Message != null)
                throw new NotFoundExceptionHandler(employees[0].Message);
            return Ok(employees);

        }

        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getAllEmployeeFromUserBranch")]
        public async Task<ActionResult<EmployeeDto>> GetAllEmployeeFromUserBranch()
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _employeeService.GetAllEmployeeFromUserBranch(decodedToken));
        }

        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getEmployeeByIdFromUserBranch")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeByIdFromUserBranch([FromQuery] int employeeId)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _employeeService.GetEmployeeByIdFromUserBranch(employeeId, decodedToken));
        }
    }
}