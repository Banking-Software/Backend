using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.ClientSetup
{
    public class ClientDto
    {
        public int? Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public int? ModificationCount { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndedOn { get; set; }
        public bool? IsKYMUpdated { get; set; }
        public string? KYMType { get; set; }
        public string AccountType { get; set; }
        public string ClientType { get; set; }
        public string? ShareType { get; set; }
        public ClientInfoDto ClientInfo { get; set; }
        public ClientContactDto? ClientContact { get; set; }
        public ClientAddressDto ClientAddress { get; set; }
        public ClientFamilyDto ClientFamily { get; set; }
        public ClientNomineeDto? ClientNominee { get; set; }
       
    }
}