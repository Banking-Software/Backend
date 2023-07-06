using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFinance.Models.UserManagement
{
    public class Employee
    {
        // START: Required Fields
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string BranchCode { get; set; }
        // Optional Field
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfJoining { get; set; }
        public string? Designation { get; set; }
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
        public string? AttachedDocumentUrlLink { get; set; }
        public string? ProfilePictureUrlLink {get; set;}
        public User User { get; set; }
    }
}