using System.ComponentModel.DataAnnotations;
using MicroFinance.Enums.Client;

namespace MicroFinance.Dtos.ClientSetup
{
    public class CreateClientDto
    {
        [Required]
        public bool IsKYMUpdated { get; set; }
        [Required]
        public string RegistrationDate { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsShareAllowed { get; set; }
        [Required]
        public ClientTypeEnum ClientType { get; set; }
        public ShareTypeEnum? ShareType { get; set; }
        public int? ClientGroupId { get; set; }
        public int? ClientUnitId { get; set; }
        public KYMTypeEnum? KYMType { get; set; }
        // Address //
        public string? PermanentVdcMunicipality { get; set; }
        public string? PermanentVdcMunicipalityNepali { get; set; }
        public string? PermanentToleVillage { get; set; }
        public string? PermanentToleVillageNepali { get; set; }
        public string? PermanentWardNumber { get; set; }
        public string? PermanentWardNumberNepali { get; set; }
        [Range(1,77, ErrorMessage ="Invalid District Code")]
        public int? PermanentDistrictCode { get; set; }
        [Range(1,7, ErrorMessage = "Invalid State Code")]
        public int? PermanentStateCode {get;set;}
        public string? TemporaryVdcMunicipality { get; set; }
        public string? TemporaryVdcMunicipalityNepali { get; set; }
        public string? TemporaryToleVillage { get; set; }
        public string? TemporaryToleVillageNepali { get; set; }
        public string? TemporaryWardNumber { get; set; }
        public string? TemporaryWardNumberNepali { get; set; }
        [Range(1,77, ErrorMessage ="Invalid District Code")]
        public int? TemporaryDistrictCode { get; set; }
        [Range(1,7, ErrorMessage = "Invalid State Code")]
        public int? TemporaryStateCode { get; set; }
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
        [Range(1,45, ErrorMessage = "Invalid Cast Code")]
        public int? ClientCastCode { get; set; }
        [Range(1,2, ErrorMessage = "Invalid Gender Code")]
        public int? ClientGenderCode { get; set; }
        public string? ClientDateOfBirth { get; set; }
        public string? ClientOccupation { get; set; }
        public string? ClientCitizenshipNumber { get; set; }
        public string? ClientCitizenShipIssueDistrict { get; set; }
        public string? ClientCitizenShipIssueDate { get; set; }
        public string? ClientNationality { get; set; }
        public string? ClientPanNumber { get; set; }
        public string? ClientEducationStatus { get; set; }
        [Range(1, 6, ErrorMessage = "Invalid Martial Status Code")]
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

        public IFormFile? ClientPhoto { get; set; }
        public IFormFile? ClientCitizenshipPhoto { get; set; }
        public IFormFile? NomineePhoto { get; set; }
        public IFormFile? ClientSignaturePhoto { get; set; }

    }
}