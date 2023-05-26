namespace MicroFinance.Dtos.UserManagement
{

    public class ActiveDeactiveInformation
    {
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public string? Message { get; set; }
        public bool? Status { get; set; }
    }
    public class ActivateDeactivateUserDto
    {
        public string UserName { get; set; }
        public bool IsActive { get; set; }
    }
}