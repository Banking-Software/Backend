using System.ComponentModel.DataAnnotations;
using MicroFinance.Enums;
using MicroFinance.Enums.Deposit.Account;

namespace MicroFinance.Dtos.DepositSetup
{
    public class UpdateDepositAccountDto
    {
        public int Id { get; set; }
        public decimal InterestRate { get; set; }
        public AccountStatusEnum Status { get; set; }
        public int? InterestPostingAccountId { get; set; }
        public int? MatureInterestPostingAccountId { get; set; }
        public string? Description { get; set; }
        public string? NomineeName { get; set; }
        public RelationTypeEnum? Relation { get; set; }
        public bool IsSMSServiceActive { get; set; }
        public int? ExpectedDailyDepositAmount { get; set; }
        public int? ExpectedTotalDepositDay { get; set; }
        public int? ExpectedTotalDepositAmount { get; set; }
        public int? ExpectedTotalReturnAmount { get; set; }
        public int? ExpectedTotalInterestAmount { get; set; }
    }
}