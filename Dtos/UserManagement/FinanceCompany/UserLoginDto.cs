namespace MicroFinance.Dtos.UserManagement
{
    public class UserLoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool StayLogin { get; set; }
    }
}