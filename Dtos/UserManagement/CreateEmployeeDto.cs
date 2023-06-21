using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.UserManagement
{
    public class CreateEmployeeDto
    {
        // START: Required Fields
        public string Name { get; set; } // Name of Employee
        public string Email { get; set; } // Email of Employee
        public string PhoneNumber { get; set; } // Employee Cell Number
        public string BranchCode { get; set; }

        // END

        //START: Optional Fields
        public string? Designation { get; set; } // Post with in company

        [DataType(DataType.Date)]
        public DateTime? DateOfJoining { get; set; } // Employee Date of joining in the company
        public string? Gender { get; set; } 
        public bool? PFAllowed { get; set; } 
        public string? SalaryPostingAccount { get; set; }
        public string? ProvidentPostingAccount { get; set; }
        public double? SalaryAmount { get; set; }
        public float? Tax { get; set; }
        public string? Facilities { get; set; }
        public string? OtherFacilities { get; set; }
        public string? Grade { get; set; }
        public string? PANNumber { get; set; }
    }
}