using System.ComponentModel.DataAnnotations;
using MicroFinance.Enums;

namespace MicroFinance.Dtos.UserManagement
{
    public class EmployeeDto
    {
        public string? Message { get; set; }
        public int? Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? BranchCode { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfJoining { get; set; }
        public string? Designation { get; set; }
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
        public string? ProfilePicFileData { get; set; }
        public string? ProfilePicFileName { get; set; }
        public FileType? ProfilePicFileType { get; set; }
        public string? CitizenShipFileData { get; set; }
        public string? CitizenShipFileName { get; set; }
        public FileType? CitizenShipFileType { get; set; }
         public string? SignatureFileData { get; set; }
        public string? SignatureFileName { get; set; }
        public FileType? SignatureFileType { get; set; }

    }
}