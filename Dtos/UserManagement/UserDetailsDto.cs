namespace MicroFinance.Dtos.UserManagement
{
    public class UserDetailsDto
    {
        // START: Employee Details
        public EmployeeDto? EmployeeData { get; set; }
        // END

        //START: If User Has Login Credentials
        public UserDto? UserData {get; set;}
        //END

        public string? Message { get; set; }
    }
}