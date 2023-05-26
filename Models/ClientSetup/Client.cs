using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.UserManagement;

namespace MicroFinance.Models.ClientSetup
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime RegistrationDate { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreateOn { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndedOn { get; set; }
        public bool? IsModified { get; set; }
        public string? ModifiedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ModifiedOn { get; set; }
        public int? ModificationCount { get; set; }
        public virtual ClientInfo ClientInfo { get; set; }
        [ForeignKey("ClientInfo")]
        public int ClientInfoId { get; set; }
        public virtual ClientAddressInfo ClientAddressInfo { get; set; }
        [ForeignKey("ClientAddressInfo")]
        public int ClientAddressInfoId { get; set; }

        public virtual ClientFamilyInfo ClientFamilyInfo { get; set; }
        [ForeignKey("ClientFamilyInfo")]
        public int ClientFamilyInfoId { get; set; }

        public virtual ClientContactInfo? ClientContactInfo { get; set; }
        [ForeignKey("ClientContactInfo")]
        public int? ClientContactInfoId { get; set; }

        public virtual ClientNomineeInfo? ClientNomineeInfo { get; set; }
        [ForeignKey("ClientNomineeInfo")]
        public int? ClientNomineeInfoId { get; set; }
        public int? ClientKYMTypeInfoId { get; set; }
        
        public int ClientAccountTypeInfoId { get; set; }
        public int ClientTypeInfoId { get; set; }
        public int? ClientShareTypeInfoId { get; set; }

        public bool? IsKYMUpdated { get; set; }

        public virtual ICollection<DepositAccount> DepositAccountSelf { get; set; }
        public virtual ICollection<DepositAccount> DepositAccountJoint { get; set; }
    }
}