using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MicroFinance.Models.UserManagement
{
    public class SuperAdmin:IdentityUser
    {
        [Required]
        public string? Name { get; set; }
        public bool IsActive { get; set; }=true;
    }
}