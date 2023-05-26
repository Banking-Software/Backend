using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.UserManagement;
using MicroFinance.Exceptions;
using MicroFinance.Models.UserManagement;
using MicroFinance.Repository.UserManagement;
using MicroFinance.Token;
using Microsoft.AspNetCore.Identity;

namespace MicroFinance.Services.UserManagement
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public EmployeeService
        (
            IEmployeeRepository employeeRepo,
            ILogger<EmployeeService> logger,
            IMapper mapper,
            ITokenService tokenService
        )
        {
            _employeeRepo = employeeRepo;
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        // START: Authorized User Service

        public async Task<TokenResponseDto> LoginService(UserLoginDto userLoginDto)
        {
            var user = await _employeeRepo.GetUserByUsername(userLoginDto.UserName);
            if (user != null)
            {
                var loginResult = await _employeeRepo.Login(user, userLoginDto.Password, userLoginDto.StayLogin);
                if (loginResult.Succeeded)
                {
                    _logger.LogInformation($"{DateTime.Now} User Logged In > {userLoginDto.UserName}");
                    var tokenData = new TokenDto()
                    {
                        UserName = user.UserName,
                        UserId = user.Id,
                        Role = await _employeeRepo.GetRole(user),
                        IsActive = user.IsActive.ToString()
                    };
                    var token = _tokenService.CreateToken(tokenData);
                    return new TokenResponseDto() { Token = token };
                }
                _logger.LogError($"{DateTime.Now} Invalid Login {loginResult.ToString()} > {userLoginDto.UserName}");
            }
            _logger.LogError($"{DateTime.Now} Attempting to login with invalid user > {userLoginDto.UserName}");
            throw new UnAuthorizedExceptionHandler("UnAuthorized");
        }

        public async Task<ResponseDto> RegisterService(UserRegisterDto userRegisterDto)
        {
            var employee = await _employeeRepo.GetEmployeeByUsername(userRegisterDto.UserName);
            if (employee == null)
                throw new NotImplementedExceptionHandler("Create Employee before creating login credentials");
                
            var user = _mapper.Map<User>(userRegisterDto);
            user.Employee = employee;
            var userCredentials =
            await _employeeRepo.Register(user, userRegisterDto.Password, userRegisterDto.Role.ToString());
            _logger.LogInformation($"{DateTime.Now}: User Login Credentails Created > {userRegisterDto.UserName}");
            return new ResponseDto()
            {
                Message = "User Credentials Created",
                StatusCode = "200",
                Status = true
            };
        }

        public async Task<string> GetRole(string id)
        {
            var user = await _employeeRepo.GetUserById(id);
            var role = await _employeeRepo.GetRole(user);
            return role;
        }

        public async Task<ResponseDto> UpdatePasswordService(UpdateUserPasswordDto updateUserPasswordDto, string userName)
        {
            var user = await _employeeRepo.GetUserByUsername(userName);
            if (user != null)
            {
                var updateStatus = await _employeeRepo
                .UpdatePassword(user, updateUserPasswordDto.OldPassword, updateUserPasswordDto.NewPassword);
                if (updateStatus)
                {
                    return new ResponseDto()
                    {
                        Message = "Update Successfull",
                        StatusCode = "200",
                        Status = true
                    };
                }
                _logger.LogError($"{DateTime.Now}: Attempting to Update Password with invalid credentials > {userName}");
                throw new BadRequestExceptionHandler("Update Failed");
            }
            _logger.LogError($"{DateTime.Now}: Attempting to Update Password of invalid user > {userName}");
            throw new UnAuthorizedExceptionHandler("UnAuthorized");
        }

        public async Task<ResponseDto> UpdateUserProfileService(UserProfileUpdateDto userProfileUpdateDto)
        {
            var user = await _employeeRepo.GetUserByUsername(userProfileUpdateDto.UserName);
            if (user != null)
            {
                user.DepositLimit = userProfileUpdateDto.DepositLimit;
                user.LoanLimit = userProfileUpdateDto.LoanLimit;
                var statusUpdate = await _employeeRepo.UpdateUserProfile(user);
                if (statusUpdate.Succeeded)
                {
                    return new ResponseDto()
                    {
                        Message = "Profile Updated Successfully",
                        StatusCode = "200",
                        Status = true
                    };
                }
                throw new BadRequestExceptionHandler($"Update Failed due to {statusUpdate.Errors}");
            }
            _logger.LogError($"{DateTime.Now}: Attemping on unknown user > {userProfileUpdateDto.UserName}");
            throw new UnAuthorizedExceptionHandler("UnAuthorized");
        }

        public async Task<ResponseDto> DeleteUser(string userName)
        {
            var user = await _employeeRepo.GetUserByUsername(userName);
            if (user != null)
            {
                var response = await _employeeRepo.DeleteUser(user);
                if (response.Succeeded)
                {
                    return new ResponseDto()
                    {
                        Message = "User Removed",
                        StatusCode = "200",
                        Status = true,
                    };
                }
                return new ResponseDto()
                {
                    Message = $"User Removed failed: {response.Errors}",
                    StatusCode = "200",
                    Status = false,
                };
            }
            throw new ApplicationException("User Doesnot Exist");
        }


        // User with Credentials every mapped data
        public async Task<UserDetailsDto> GetUserDetailsByIdService(string id)
        {
            var user = await _employeeRepo.GetUserDetailsById(id);
            if (user == null)
            {
                _logger.LogError($"{DateTime.Now}: Attempting to acces unknown user with id > {id}");
               throw new UnAuthorizedExceptionHandler("UnAuthorized");
            }
            var userDto = _mapper.Map<UserDto>(user);
            Employee employee = user.Employee;
            EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);
            var userRole = await _employeeRepo.GetRole(user);
            userDto.Role = userRole;
            UserDetailsDto userDetailsDto = new UserDetailsDto()
            {
                Message = "Success",
                EmployeeData = employeeDto,
                UserData = userDto
            };
            return userDetailsDto;
        }
        // User with Credentials every mapped data
        public async Task<UserDetailsDto> GetUserDetailsByUserNameService(string userName)
        {
            var user = await _employeeRepo.GetUserByUsername(userName);
            if (user == null)
            {
                _logger.LogError($"{DateTime.Now}: Attempting to acces unknown user > {userName}");
               throw new UnAuthorizedExceptionHandler("UnAuthorized");
            }
            var userDto = _mapper.Map<UserDto>(user);
            Employee employee = user.Employee;
            EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);
            var userRole = await _employeeRepo.GetRole(user);
            userDto.Role = userRole;
            UserDetailsDto userDetailsDto = new UserDetailsDto()
            {
                Message = "Success",
                EmployeeData = employeeDto,
                UserData = userDto
            };
            return userDetailsDto;

        }

        // Only details related to user credentials
        public async Task<UserDto> GetUserByIdService(string id)
        {
            var user = await _employeeRepo.GetUserDetailsById(id);
            if (user == null)
            {
                _logger.LogError($"{DateTime.Now}: Attempting to acces unknown user with id > {id}");
               throw new UnAuthorizedExceptionHandler("UnAuthorized");
            }
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Message = "Success";
            return userDto;
        }
        // Only details related to user credentials

        public async Task<UserDto> GetUserByUserNameService(string userName)
        {
            var user = await _employeeRepo.GetUserByUsername(userName);
            if (user == null)
            {
                _logger.LogError($"{DateTime.Now}: Attempting to acces unknown user> {userName}");
               throw new UnAuthorizedExceptionHandler("UnAuthorized");
            }
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Message = "Success";
            return userDto;
        }

        // User with Credentials every mapped data
        public async Task<List<UserDetailsDto>> GetUsersDetailsService()
        {
            var users = await _employeeRepo.GetUsers();
            List<UserDetailsDto> userDetailsDtos = new List<UserDetailsDto>();
            if (users.Count <= 0 || users == null)
            {
                userDetailsDtos.Add(
                    new UserDetailsDto()
                    {
                        Message = "No Data Found"
                    });
                return userDetailsDtos;
            }
            foreach (var user in users)
            {
                var userDto = _mapper.Map<UserDto>(user);
                Employee employee = user.Employee;
                EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);
                var userRole = await _employeeRepo.GetRole(user);
                userDto.Role = userRole;
                UserDetailsDto userDetailsDto = new UserDetailsDto()
                {
                    EmployeeData = employeeDto,
                    UserData = userDto
                };
                userDetailsDtos.Add(userDetailsDto);
            }
            return userDetailsDtos;
        }
        
        public async Task<ResponseDto> AssignRoleService(string userName, string role)
        {
            var user = await _employeeRepo.GetUserByUsername(userName);
            if (user != null)
            {
                var status = await _employeeRepo.AssignRole(user, role);
                if (status.Succeeded)
                {
                    _logger.LogInformation($"{DateTime.Now} new role '{role}' assiged to > {userName}");
                    return new ResponseDto()
                    {
                        Message = "Role Assignment Successfull",
                        StatusCode = "200",
                        Status = true
                    };
                }
                _logger.LogError($"{DateTime.Now} Role assignment failed due to {status.Errors} > {userName}");
                return new ResponseDto()
                {
                    Message = $"Assignment Failed. {status.Errors}",
                    StatusCode = "400",
                    Status = false
                };
            }
            return new ResponseDto()
            {
                Message = $"Assignment Failed",
                StatusCode = "404",
                Status = false
            };
        }

        // END


        // START: Employee Service
        public async Task<ResponseDto> CreateEmployeeService(CreateEmployeeDto createEmployeeDto)
        {
            var userStaff = await _employeeRepo.GetEmployeeByUsername(createEmployeeDto.UserName);
            if (userStaff != null)
                throw new BadRequestExceptionHandler("Employee Already Exist");

            var employee = _mapper.Map<Employee>(createEmployeeDto);
            var newUserStaff = await _employeeRepo.CreateEmployee(employee);
            if (newUserStaff >= 1)
            {
                _logger.LogInformation($"{DateTime.Now} New Employee Created > {createEmployeeDto.UserName}");
                return new ResponseDto()
                {
                    Message = "Employee Created",
                    StatusCode = "200",
                    Status = true,
                };
            }
            _logger.LogError($"{DateTime.Now} New Employee Creation failed> {createEmployeeDto.UserName}");
            throw new BadRequestExceptionHandler("Employee Creation Failed");

        }

        public async Task<ResponseDto> EditProfileService(CreateEmployeeDto createEmployeeDto)
        {
            
            var employee = _mapper.Map<Employee>(createEmployeeDto);
            var editStatus = await _employeeRepo.EditEmployeeProfile(employee);
            if (editStatus >= 1)
            {
                var responseDto = new ResponseDto();
                _logger.LogInformation($"{DateTime.Now} Employee Profile Updated: {createEmployeeDto.UserName}");
                responseDto.Message = "Update Successfull";
                responseDto.StatusCode = "200";
                responseDto.Status = true;
                return responseDto;
            }

            throw new BadRequestExceptionHandler("Failed to update profile");
        }

        public async Task<EmployeeDto> GetEmployeeById(int id)
        {
            var employee = await _employeeRepo.GetEmployeeById(id);
            if (employee == null) 
                throw new UnAuthorizedExceptionHandler("UnAuthorized");

            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            employeeDto.Message = "Success";
            return employeeDto;
        }

        public async Task<EmployeeDto> GetEmployeeByUserName(string userName)
        {
            var employee = await _employeeRepo.GetEmployeeByUsername(userName);
            if (employee == null) 
                throw new UnAuthorizedExceptionHandler("UnAuthorized");
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            employeeDto.Message = "Success";
            return employeeDto;

        }

        public async Task<List<EmployeeDto>> GetEmployess()
        {
            var employees = await _employeeRepo.GetEmployees();
            if (employees.Count <= 0 || employees == null)
            {
                return new List<EmployeeDto>()
                {
                    new EmployeeDto(){Message="Not Data Found"}
                };
            }
            var employeeDtos = _mapper.Map<List<EmployeeDto>>(employees);
            return employeeDtos;

        }


        public async Task<ResponseDto> DeleteEmployee(string userName)
        {
            var user = await _employeeRepo.GetUserByUsername(userName);
            if (user == null)
            {
                var employee = await _employeeRepo.GetEmployeeByUsername(userName);
                if (employee != null)
                {
                    var response = await _employeeRepo.DeleteEmployee(employee);
                    if (response >= 1)
                    {
                        return new ResponseDto()
                        {
                            Message = "Employee Deleted",
                            StatusCode = "200",
                            Status = true
                        };
                    }
                }
                return new ResponseDto()
                {
                    Message = "Employee Delete Failed.",
                    StatusCode = "500",
                    Status = false
                };
            }
            return new ResponseDto()
            {
                Message = "First Remove User Credentials before deleting Employee",
                StatusCode = "403",
                Status = false
            };
        }


        // END
    }
}