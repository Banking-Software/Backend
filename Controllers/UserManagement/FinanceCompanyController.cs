using System.Reflection;
using System.Security.Claims;
using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.UserManagement;
using MicroFinance.ErrorManage;
using MicroFinance.Exceptions;
using MicroFinance.Role;
using MicroFinance.Services;
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

        public FinanceCompanyController
        (
            IEmployeeService employeeService,
            ILogger<FinanceCompanyController> logger,
            IMapper mapper,
            ITokenService tokenService
        )
        {
            _employeeService = employeeService;
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
        }


        // START: API for Authorized user //
        [TypeFilter(typeof(IsUserOfficerFilter))]
        [HttpPost("register")]
        public async Task<ActionResult<ResponseDto>> Register(UserRegisterDto userRegisterDto)
        {

            if (userRegisterDto.IsActive)
                throw new UnAuthorizedExceptionHandler("You are not authorized to activate User");

            return Ok(await _employeeService.RegisterService(userRegisterDto));

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenResponseDto>> Login(UserLoginDto userLoginDto)
        {

            var user = await _employeeService.GetUserByUserNameService(userLoginDto.UserName);
            if (user.IsActive == false)
                throw new UnAuthorizedExceptionHandler("Unauthorized Access. Contact Officer");
            var tokenResponseDto = await _employeeService.LoginService(userLoginDto);
            return Ok(tokenResponseDto);

        }

        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpPut("update-password")]
        public async Task<ActionResult<ResponseDto>> UpdatePassword(UpdateUserPasswordDto updateUserPasswordDto)
        {

            var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            var updateStatus = await _employeeService.UpdatePasswordService(updateUserPasswordDto, userName);
            return Ok(updateStatus);

        }

        [TypeFilter(typeof(IsUserOfficerFilter))]
        [HttpPut("update-profile")]
        public async Task<ActionResult<ResponseDto>> UpdateUserProfile(UserProfileUpdateDto userProfileUpdateDto)
        {

            return Ok(await _employeeService.UpdateUserProfileService(userProfileUpdateDto));

        }


        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getuser/username")]
        public async Task<ActionResult<UserDetailsDto>> GetUserByUserName([FromQuery] string userName)
        {

            var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var currentUserName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            if (role != UserRole.Officer.ToString() && currentUserName != userName)
                throw new UnAuthorizedExceptionHandler("Not Authorized to view");
            var user = await _employeeService.GetUserDetailsByUserNameService(userName);
            if (user.Message.ToString() != "Success")
                throw new NotFoundExceptionHandler(user.Message);
            return Ok(user);
        }


        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getuser/id")]
        public async Task<ActionResult<UserDetailsDto>> GetUserById([FromQuery] string id)
        {

            var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (role != UserRole.Officer.ToString() && currentUserId != id)
                throw new UnAuthorizedExceptionHandler("Not Authorized to view");
            var user = await _employeeService.GetUserDetailsByIdService(id);
            if (user.Message.ToString() != "Success")
                throw new NotFoundExceptionHandler(user.Message);
            return Ok(user);

        }


        [TypeFilter(typeof(IsUserOfficerFilter))]
        [HttpGet("getusers")]
        public async Task<ActionResult<List<UserDetailsDto>>> GetUsers()
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
        [HttpPost("assignrole")]
        public async Task<ActionResult<ResponseDto>> AssignRole(AssignRoleDto assignRoleDto)
        {

            var response = await _employeeService.AssignRoleService(assignRoleDto.UserName, assignRoleDto.Role.ToString());
            return response;

        }




        // END // 

        // START: API for company Employee //

        [TypeFilter(typeof(IsUserOfficerFilter))]
        [HttpPost("create-employee")]
        public async Task<ActionResult<ResponseDto>> CreateEmployee(CreateEmployeeDto createEmployeeDto)
        {


            var userCreate = await _employeeService.CreateEmployeeService(createEmployeeDto);
            return Ok(userCreate);

        }


        [TypeFilter(typeof(IsUserOfficerFilter))]
        [HttpPut("edit-profile")]
        public async Task<ActionResult<ResponseDto>> UpdateEmployeeProfile(CreateEmployeeDto createEmployeeDto)
        {
            return Ok(await _employeeService.EditProfileService(createEmployeeDto));
        }

        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getemployee/userName")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeByUserName([FromQuery] string userName)
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var currentUserName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            if (role != UserRole.Officer.ToString() && currentUserName != userName)
                throw new UnAuthorizedExceptionHandler("Not Authorized to view");

            var employee = await _employeeService.GetEmployeeByUserName(userName);
            if (employee.Message != "Success")
                throw new NotFoundExceptionHandler(employee.Message);
            return Ok(employee);
        }



        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getemployee/id")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById([FromQuery] int id)
        {

            var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var employee = await _employeeService.GetEmployeeById(id);
            if (employee.Message != "Success")
                throw new NotFoundExceptionHandler(employee.Message);

            var user = await _employeeService.GetUserByUserNameService(employee.UserName);

            if (role != UserRole.Officer.ToString() && currentUserId != user.UserId)
                throw new UnAuthorizedExceptionHandler("Not Authorized to view");

            return Ok(employee);

        }


        [TypeFilter(typeof(IsUserOfficerFilter))]
        [HttpGet("getemployees")]
        public async Task<ActionResult<List<EmployeeDto>>> GetEmployees()
        {

                var employees = await _employeeService.GetEmployess();
                if (employees.Count <= 1 && employees[0].Message != null) 
                    throw new NotFoundExceptionHandler(employees[0].Message);

                return Ok(employees);
            
        }

        // [TypeFilter(typeof(IsUserOfficerFilter))]
        // [HttpGet("deleteemployee")]
        // public async Task<ActionResult<ResponseDto>> DeleteEmployee([FromQuery] string username)
        // {
        //     try
        //     {
        //         var currentUserName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
        //         if (currentUserName == username)
        //             return Unauthorized(new ApiResponses(401, "You are authorized to remove yourself"));
        //         var response = await _employeeService.DeleteEmployee(username);
        //         return response;
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError($"{DateTime.Now} (FinanceCompany-DeleteUser) Exception: {ex.Message}");
        //         return BadRequest(new ApiResponses(400, "Bad Request"));
        //     }
        // }
        // END
    }
}