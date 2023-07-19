using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.UserManagement
{
    public class UserProfileUpdateDto
    {
        [Required]
        public string UserName { get; set; }
        public double? DepositLimit { get; set; }
        public double? LoanLimit { get; set; }

    }
}