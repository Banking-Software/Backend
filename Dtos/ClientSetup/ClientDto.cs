using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.ClientSetup
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatorBranchCode { get; set; }
        public string CreatorId { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool? IsModified { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifierId { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModificationCount { get; set; }
        public bool IsKYMUpdated { get; set; }
        public bool IsShareAllowed { get; set; }
        public DateTime RegistrationDate { get; set; }
        // Foreign Key
        public int ClientTypeId { get; set; }
        public string ClientType { get; set; }
        public int? ClientShareTypeInfoId { get; set; }
        public string? ShareType { get; set; }
        public int? ClientGroupId { get; set; }
        public string? ClientGroup { get; set; }
        public int? ClientUnitId { get; set; }
        public string? ClientUnit { get; set; }
        public int? KYMTypeId { get; set; }
        public string? KYMType { get; set; }
        // Foreign Key ended
        // Address //
        public string? PermanentVdcMunicipality { get; set; }
        public string? PermanentVdcMunicipalityNepali { get; set; }
        public string? PermanentToleVillage { get; set; }
        public string? PermanentToleVillageNepali { get; set; }
        public string? PermanentWardNumber { get; set; }
        public string? PermanentWardNumberNepali { get; set; }
        public string? PermanentDistrict { get; set; }
        public int? PermanentDistrictCode { get; set; }
        public string? PermanentDistrictNepali { get; set; }
        public string? PermanentState { get; set; }
        public int? PermanentStateCode {get;set;}
        public string? TemporaryVdcMunicipality { get; set; }
        public string? TemporaryVdcMunicipalityNepali { get; set; }
        public string? TemporaryToleVillage { get; set; }
        public string? TemporaryToleVillageNepali { get; set; }
        public string? TemporaryWardNumber { get; set; }
        public string? TemporaryWardNumberNepali { get; set; }
        public string? TemporaryDistrict { get; set; }
        public int? TemporaryDistrictCode { get; set; }
        public string? TemporaryDistrictNepali { get; set; }
        public string? TemporaryState { get; set; }
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
        public string ClientFirstName { get; set; }
        public string? ClientMiddleName { get; set; }
        public string ClientLastName { get; set; }
        public string? ClientNepaliName { get; set; }
        public string? ClientCast { get; set; }
        public int? ClientCastCode { get; set; }
        public string? ClientGender { get; set; }
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
        public string? ClientMaritalStatus { get; set; }
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
       
    }
}