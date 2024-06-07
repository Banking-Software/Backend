using MicroFinance.Dtos.ClientSetup;
using MicroFinance.Dtos.DepositSetup.Account;
using MicroFinance.Enums;
using MicroFinance.Enums.Deposit.Account;

namespace MicroFinance.Dtos.DepositSetup
{
    public class DepositAccountDto : BaseDepositDto
    {
        public string AccountNumber { get; set; }
        public string NepaliCreationDate { get; set; }
        public DateTime EnglishCreationDate { get; set; }
        // public string NepaliOpeningDate { get; set; }
        public int Period { get; set; }
        public PeriodTypeEnum PeriodType { get; set; }
        public AccountTypeEnum AccountType { get; set; }
        public string NepaliMatureDate { get; set; }
        public DateTime EnglishMatureDate { get; set; }
        public DateTime NextInterestPostingDate {get; set;}
        public decimal InterestRate { get; set; }
        public bool IsWithDrawalAllowed { get; set; }
        public decimal PrincipalAmount { get; set; } = 0;
        public decimal InterestAmount { get; set; } = 0;
        public int ReferredByEmployeeId { get; set; }
        public string ReferredByEmployeeName { get; set; }
        public RelationTypeEnum? Relation { get; set; }
        public string? NomineeName { get; set; }
        public string? Description { get; set; }
        public AccountStatusEnum Status { get; set; }
        public bool IsSMSServiceActive { get; set; }
        public int? ExpectedDailyDepositAmount { get; set; }
        public int? ExpectedTotalDepositDay { get; set; }
        public int? ExpectedTotalDepositAmount { get; set; }
        public int? ExpectedTotalReturnAmount { get; set; }
        public int? ExpectedTotalInterestAmount { get; set; }
        public string? InterestPostingAccountNumber { get; set; }
        public string? MatureInterestPostingAccountNumber { get; set; }
        public string? SignatureFileData { get; set; }
        public string? SignatureFileName { get; set; }
        public FileType? SignatureFileType { get; set; }
    }
}