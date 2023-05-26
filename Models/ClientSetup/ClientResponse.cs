using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroFinance.Models.UserManagement;

namespace MicroFinance.Models.ClientSetup
{
    public class ClientResponse
    {

        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string CreatedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreateOn { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndedOn { get; set; }
        public bool? IsModified { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModificationCount { get; set; }
        public bool? IsKYMUpdated { get; set; }
        public ClientInfo ClientInfo { get; set; }
        public ClientAddressInfo ClientAddressInfo { get; set; }

        public ClientFamilyInfo ClientFamilyInfo { get; set; }

        public ClientContactInfo? ClientContactInfo { get; set; }

        public ClientNomineeInfo? ClientNomineeInfo { get; set; }
        public string? KYMType { get; set; }
        public string AccountType { get; set; }
        public string ClientType { get; set; }
        public string? ShareType { get; set; }

        
    }
}