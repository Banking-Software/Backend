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
        public string CreatedBy { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

    }
}