using System.Reflection;
using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.UserManagement;
using MicroFinance.Enums;
using MicroFinance.Exceptions;
using MicroFinance.Models.UserManagement;
using MicroFinance.Repository.UserManagement;
using MicroFinance.Services.CompanyProfile;
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
        private readonly ICompanyProfileService _companyProfile;
        private readonly IConfiguration _config;

        public EmployeeService
        (
            IEmployeeRepository employeeRepo,
            ILogger<EmployeeService> logger,
            IMapper mapper,
            ITokenService tokenService,
            ICompanyProfileService companyProfile,
            IConfiguration config
        )
        {
            _employeeRepo = employeeRepo;
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
            _companyProfile = companyProfile;
            _config = config;
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
                        IsActive = user.IsActive.ToString(),
                        Email = user.Email,
                        BranchCode = user.Employee.BranchCode
                    };
                    var token = _tokenService.CreateToken(tokenData);
                    return new TokenResponseDto() { Token = token };
                }
                _logger.LogError($"{DateTime.Now} Invalid Login {loginResult.ToString()} > {userLoginDto.UserName}");
            }
            _logger.LogError($"{DateTime.Now} Attempting to login with invalid user > {userLoginDto.UserName}");
            throw new UnAuthorizedExceptionHandler("UnAuthorized");
        }

        public async Task<ResponseDto> RegisterService(UserRegisterDto userRegisterDto, string createdBy)
        {
            var employee = await _employeeRepo.GetEmployeeByEmail(userRegisterDto.Email);
            if (employee == null)
                throw new NotImplementedExceptionHandler("Create Employee before creating login credentials");

            var user = _mapper.Map<User>(userRegisterDto);
            user.Employee = employee;
            user.CreatedBy = createdBy;
            user.CreatedOn = DateTime.Now;
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

        public async Task<ResponseDto> UpdateUserProfileService(UserProfileUpdateDto userProfileUpdateDto, string modifiedBy)
        {
            var user = await _employeeRepo.GetUserByUsername(userProfileUpdateDto.UserName);
            if (user != null)
            {
                user.DepositLimit = userProfileUpdateDto.DepositLimit;
                user.LoanLimit = userProfileUpdateDto.LoanLimit;
                user.ModifiedBy = modifiedBy;
                user.ModifiedOn = DateTime.Now;
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

        public async Task<UserDetailsDto> GetUserDetailsByEmailService(string email)
        {
            var user = await _employeeRepo.GetUserByEmail(email);
            if (user == null)
            {
                _logger.LogError($"{DateTime.Now}: Attempting to acces unknown user > {email}");
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
        public async Task<UserDto> GetUserByEmailService(string email)
        {
            var user = await _employeeRepo.GetUserByEmail(email);
            if (user == null)
            {
                _logger.LogError($"{DateTime.Now}: Attempting to acces unknown user> {email}");
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
        private static void SetPropertyByName(object obj, string propertyName, object newValue)
        {
            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null && propertyInfo.CanWrite)
                propertyInfo.SetValue(obj, newValue);
            else
                throw new Exception($"Property '{propertyName}' not found or not writable.");
        }

        private Task<Employee> UploadImage(Employee employee, IFormFile? image, List<string> listOfPropertyName)
        {
            if(image==null) 
                return Task.FromResult(employee);
            string fileExtenstion = (Path.GetExtension(image.FileName)).Replace(".", "").ToUpper();
            try
            {
                float maxFileValue = float.Parse(_config["ApplicationSettings:ImageMaxSize"]);
                double maxFileSize = maxFileValue * 1024 * 1024; // 3MB 
                if (image.Length > maxFileSize)
                    throw new Exception($"File size exceeded the Limit. Upto {maxFileSize}MB is allowed while {image.Length}MB is received");
                var fileType = (FileType)Enum.Parse(typeof(FileType), fileExtenstion);
                using (var stream = new MemoryStream())
                {
                    image.CopyTo(stream);
                    //employee.GetType().GetProperty(listOfPropertyName[0]).SetValue(employee, stream.ToArray);
                    SetPropertyByName(employee, listOfPropertyName[0], stream.ToArray());
                    SetPropertyByName(employee, listOfPropertyName[1], image.FileName);
                    SetPropertyByName(employee, listOfPropertyName[2], fileType);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Given Image Format is wrong. {ex.Message}");
            }
            return Task.FromResult(employee);
        }
        public async Task<ResponseDto> CreateEmployeeService(CreateEmployeeDto createEmployeeDto, string createdBy)
        {
            var userStaff = await _employeeRepo.GetEmployeeByEmail(createEmployeeDto.Email);
            if (userStaff != null)
                throw new BadRequestExceptionHandler("Employee Already Exist");

            var branch = await _companyProfile.GetBranchServiceByBranchCodeService(createEmployeeDto.BranchCode);
            var company = await _companyProfile.GetCompanyProfileService();
            if (branch == null || !branch.IsActive || company == null)
                throw new Exception("Branch and company details should exist and they need to be active inorder to proceed.");
            
            var employee = _mapper.Map<Employee>(createEmployeeDto);
            employee.CreatedBy = createdBy;
            employee.CreatedOn = DateTime.Now;            
            List<string> listOfProfileProperty = new List<string>(){nameof(Employee.ProfilePicFileData),  nameof(Employee.ProfilePicFileName),nameof(Employee.ProfilePicFileType)};
            List<string> listOfCitizenProperty = new List<string>(){nameof(Employee.CitizenShipFileData), nameof(Employee.CitizenShipFileName),nameof(Employee.CitizenShipFileType)};
            List<string> listOfSignatureProperty = new List<string>(){nameof(Employee.SignatureFileData), nameof(Employee.SignatureFileName),nameof(Employee.SignatureFileType)};

            //if(createEmployeeDto.ProfilePic!=null)
            employee = await UploadImage(employee, createEmployeeDto?.ProfilePic,listOfProfileProperty);
            //if(createEmployeeDto.CitizenShipPic!=null)
            employee = await UploadImage(employee, createEmployeeDto?.CitizenShipPic,listOfCitizenProperty);
            //if(createEmployeeDto.SignaturePic!=null)
            employee = await UploadImage(employee, createEmployeeDto?.SignaturePic,listOfSignatureProperty);
            
            var newUserStaff = await _employeeRepo.CreateEmployee(employee);
            if (newUserStaff >= 1)
            {
                _logger.LogInformation($"{DateTime.Now} New Employee Created > {createEmployeeDto.Email}");
                return new ResponseDto()
                {
                    Message = "Employee Created",
                    StatusCode = "200",
                    Status = true,
                };
            }
            _logger.LogError($"{DateTime.Now} New Employee Creation failed> {createEmployeeDto.Email}");
            throw new BadRequestExceptionHandler("Employee Creation Failed");

        }

        private async Task<Employee> UpdateImage(UpdateEmployeeDto updateEmployeeDto, Employee updateEmployee, Employee existingEmployee)
        {
            if(updateEmployeeDto.IsProfilePicChanged)
            {
                List<string> listOfProfileProperty = new List<string>(){nameof(Employee.ProfilePicFileData),  nameof(Employee.ProfilePicFileName),nameof(Employee.ProfilePicFileType)};
                updateEmployee = await UploadImage(updateEmployee, updateEmployeeDto?.ProfilePic,listOfProfileProperty);
            }
            else
            {
                updateEmployee.ProfilePicFileData = existingEmployee.ProfilePicFileData;
                updateEmployee.ProfilePicFileName = existingEmployee.ProfilePicFileName;
                updateEmployee.ProfilePicFileType = existingEmployee.ProfilePicFileType;
            }
            if(updateEmployeeDto.IsCitizenPicChanged)
            {
                List<string> listOfCitizenProperty = new List<string>(){nameof(Employee.CitizenShipFileData), nameof(Employee.CitizenShipFileName),nameof(Employee.CitizenShipFileType)};
                updateEmployee = await UploadImage(updateEmployee, updateEmployeeDto?.CitizenShipPic,listOfCitizenProperty);

            }
            else
            {
                updateEmployee.CitizenShipFileData = existingEmployee.CitizenShipFileData;
                updateEmployee.CitizenShipFileName = existingEmployee.CitizenShipFileName;
                updateEmployee.CitizenShipFileType = existingEmployee.CitizenShipFileType;
            }
            if(updateEmployeeDto.IsSignaturePicChanged)
            {
                List<string> listOfSignatureProperty = new List<string>(){nameof(Employee.SignatureFileData), nameof(Employee.SignatureFileName),nameof(Employee.SignatureFileType)};
                updateEmployee = await UploadImage(updateEmployee, updateEmployeeDto?.SignaturePic,listOfSignatureProperty);
            }
            else
            {
                updateEmployee.SignatureFileData = existingEmployee.SignatureFileData;
                updateEmployee.SignatureFileName= existingEmployee.SignatureFileName;
                updateEmployee.SignatureFileType = existingEmployee.SignatureFileType;
            }

            return updateEmployee;
        }

        public async Task<ResponseDto> EditProfileService(UpdateEmployeeDto updateEmployeeDto, TokenDto decodedToken)
        {
            var existingEmployee =  await _employeeRepo.GetEmployeeById(updateEmployeeDto.Id);
            if(existingEmployee==null) 
                throw new Exception("Invalid Employee Request");

            var employee = _mapper.Map<Employee>(updateEmployeeDto);
            employee.Id= existingEmployee.Id;
            employee.CreatedBy = existingEmployee.CreatedBy;
            employee.CreatedOn = existingEmployee.CreatedOn;
            employee.ModifiedBy = decodedToken.UserName;
            employee.ModifiedOn = DateTime.Now;
            employee = await UpdateImage(updateEmployeeDto, employee, existingEmployee);
            var editStatus = await _employeeRepo.EditEmployeeProfile(employee, existingEmployee.Email);
            if (editStatus >= 1)
            {
                var responseDto = new ResponseDto();
                _logger.LogInformation($"{DateTime.Now} Employee Profile Updated: {updateEmployeeDto.Email}");
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

        public async Task<EmployeeDto> GetEmployeeByEmail(string email)
        {
            var employee = await _employeeRepo.GetEmployeeByEmail(email);
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




        // END
    }
}