using System.ComponentModel.DataAnnotations;
using MicroFinance.Enums;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Share;

namespace MicroFinance.Models.ClientSetup
{
    public class Client : ClientBase
    {

        [Required]
        public bool IsKYMUpdated { get; set; }

        [Required]
        public bool IsShareAllowed { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }
        // Address //
        public string? PermanentVdcMunicipality { get; set; }
        public string? PermanentVdcMunicipalityNepali { get; set; }
        public string? PermanentToleVillage { get; set; }
        public string? PermanentToleVillageNepali { get; set; }
        public string? PermanentWardNumber { get; set; }
        public string? PermanentWardNumberNepali { get; set; }
        public int? PermanentDistrictCode { get; set; }
        public int? PermanentStateCode {get;set;}
        public string? TemporaryVdcMunicipality { get; set; }
        public string? TemporaryVdcMunicipalityNepali { get; set; }
        public string? TemporaryToleVillage { get; set; }
        public string? TemporaryToleVillageNepali { get; set; }
        public string? TemporaryWardNumber { get; set; }
        public string? TemporaryWardNumberNepali { get; set; }
        public int? TemporaryDistrictCode { get; set; }
        public string? TemporaryStateCode { get; set; }
        // Address End

        // Contact //
        public string? ClientMobileNumber1 { get; set; }
        public string? ClientMobileNumber2 { get; set; }
        public string? ClientTelephoneNumber1 { get; set; }
        public string? ClientTelephoneNumber2 { get; set; }
        public string? ClientEmail { get; set; }
        public string? ClientWebsite { get; set; }
        // Contact End //

        // Family //
        public string? ClientMotherName { get; set; }
        public string? ClientMotherNameNepali { get; set; }
        public string? ClientFatherName { get; set; }
        public string? ClientFatherNameNepali { get; set; }
        public string? ClientGrandFatherName { get; set; }
        public string? ClientGrandFatherNameNepali { get; set; }
        public string? ClientSpouseName { get; set; }
        public string? ClientSpouseOccupation { get; set; }
        public string? ClientNameOfSons { get; set; }
        public string? ClientNameOfDaughters { get; set; }
        public string? ClientFatherInLaw { get; set; }
        public string? ClientMotherInLaw { get; set; }
        // Family End //

        // Client Personal Info //
        [Required]
        public string ClientFirstName { get; set; }
        public string? ClientMiddleName { get; set; }
        [Required]
        public string ClientLastName { get; set; }
        public string? ClientNepaliName { get; set; }
        public int? ClientCastCode { get; set; }
        public int? ClientGenderCode { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ClientDateOfBirth { get; set; }
        public string? ClientOccupation { get; set; }
        public string? ClientCitizenshipNumber { get; set; }
        public string? ClientCitizenShipIssueDistrict { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ClientCitizenShipIssueDate { get; set; }
        public string? ClientNationality { get; set; }
        public string? ClientPanNumber { get; set; }
        public string? ClientEducationStatus { get; set; }
        public int? ClientMartialStatusCode { get; set; }
        public string? ClientNationalityIdStatus { get; set; }
        public string? ClientVotingId { get; set; }
        public string? ClientOtherInfo { get; set; }
        public string? ClientOtherInfo2 { get; set; }
        public string? ClientIncomeSource { get; set; }
        public string? ClientAccountPurposeNepali { get; set; }
        public string? ClientIfMemberOfOtherParty { get; set; }
        // Client Personal Info Ended //

        // Client Nominee
        public string? NomineeName { get; set; }
        public string? NomineeNepaliName { get; set; }
        public string? NomineeRelation { get; set; }
        public string? NomineeNepaliRelation { get; set; }
        public string? NomineeIntroducedBy { get; set; }
        public string? NomineeAddress { get; set; }
        public string? NomineeCitizenshipNumber { get; set; }
        public string? NomineeContactNumber { get; set; }
        // Nominee Ended

        // Files Attached
        public byte[]? ClientPhotoFileData { get; set; }
        public string? ClientPhotoFileName { get; set; }
        public FileType? ClientPhotoFileType { get; set; }
        public byte[]? ClientCitizenshipFileData { get; set; }
        public string? ClientCitizenshipFileName { get; set; }
        public FileType? ClientCitizenshipFileType { get; set; }
        public byte[]? ClientSignatureFileData { get; set; }
        public string? ClientSignatureFileName { get; set; }
        public FileType? ClientSignatureFileType { get; set; }
        public byte[]? NomineePhotoFileData { get; set; }
        public string? NomineePhotoFileName { get; set; }
        public FileType? NomineePhotoFileType { get; set; }

        public virtual ClientType ClientType { get;set;}
        public int ClientTypeId { get; set; }
        public virtual Ledger? ShareType { get; set; }
        public int? ClientShareTypeInfoId { get; set; }
        public virtual ClientGroup? ClientGroup {get;set;}
        public int? ClientGroupId { get; set; }
        public virtual ClientUnit? ClientUnit { get; set; }
        public int? ClientUnitId { get; set; }
        public ClientKYMType? KYMType { get; set; }
        public int? KYMTypeId { get; set; }

        public virtual ICollection<DepositAccount> DepositAccountSelf { get; set; }
        public virtual ICollection<JointAccount> JointAccounts { get; set; }
        public virtual ShareAccount ShareAccount { get; set; }
    }
}