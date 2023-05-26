using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.UserManagement
{
    public class CreateEmployeeDto
    {
        // START: Required Fields
        public string CreatedBy { get; set; } // Stores authorized user ID whosoever created this KYC
        public string Name { get; set; } // Name of Employee
        public string Email { get; set; } // Email of Employee
        public string UserName { get; set; } // Unique Identifier for the Employee
        public string CompanyName { get; set; } // Company name where employee work
        public string BranchName { get; set; } // Branch where employee work
        public string PhoneNumber { get; set; } // Employee Cell Number

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