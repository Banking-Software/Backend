using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.ClientSetup
{
    public class ClientInfoDto
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? NepaliName { get; set; }
        public string? Cast { get; set; }
        public string? Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public string? Occupation { get; set; }
        public string CitizenshipNumber { get; set; }
        public string? IssueDistrict { get; set; }
        [DataType(DataType.Date)]
        public DateTime? IssueDate { get; set; }
        public string? Nationality { get; set; }
        public string? PanNumber { get; set; }
        public string? EducationStatus { get; set; }
        public string? MaritalStatus { get; set; }
        public string? NationalityIdStatus { get; set; }
        public string? VotingId { get; set; }
        public string? OtherInfo { get; set; }
        public string? OtherInfo2 { get; set; }
        public string? IncomeSource { get; set; }
        public string? AccountPurposeNepali { get; set; }
        public string? IfMemberOfOtherParty { get; set; }
    }
}