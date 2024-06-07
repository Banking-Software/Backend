using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.UserManagement
{
    public class CreateEmployeeDto
    {
        // START: Required Fields
        [Required]
        public string Name { get; set; } // Name of Employee
        [Required]
        public string Email { get; set; } // Email of Employee
        [Required]
        public string PhoneNumber { get; set; } // Employee Cell Number
        [Required]
        public string BranchCode { get; set; }

        // END

        //START: Optional Fields
        public string? Designation { get; set; } // Post with in company
        public string? DateOfJoining { get; set; } // Employee Date of joining in the company
        [RegularExpression(@"^[1-2]", ErrorMessage = "Please enter correct code for Gender")]
        public int? GenderCode { get; set; } 
        public bool? PFAllowed { get; set; } 
        public string? SalaryPostingAccount { get; set; }
        public string? ProvidentPostingAccount { get; set; }
        public double? SalaryAmount { get; set; }
        public float? Tax { get; set; }
        public string? Facilities { get; set; }
        public string? OtherFacilities { get; set; }
        public string? Grade { get; set; }
        public string? PANNumber { get; set; }
        public IFormFile? ProfilePic { get; set; }
        public IFormFile? CitizenShipPic { get; set; }
        public IFormFile? SignaturePic { get; set; }
        
    }
}