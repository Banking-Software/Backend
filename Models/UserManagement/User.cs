using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MicroFinance.Models.UserManagement
{
    public class User : IdentityUser
    {
        public double? DepositLimit { get; set; }
        public double? LoanLimit { get; set; }
        public bool IsActive { get; set; } = false;
        [Required]
        public string? CreatedBy { get; set; }
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

    }
}