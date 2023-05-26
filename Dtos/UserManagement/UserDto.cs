namespace MicroFinance.Dtos.UserManagement
{
    public class UserDto
    {
        public string? Message { get; set; }
        public string? UserName { get; set; }
        public string? UserId { get; set; }
        public string? Role { get; set; }
        public bool? IsActive { get; set; }
        public string? Token { get; set; }
        public string? Password { get; set; }
        public double? DepositLimit { get; set; }
        public double? LoanLimit { get; set; }
        public string? CreatedBy { get; set; }
    }
}