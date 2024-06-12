namespace MicroFinance.Models.Wrapper.Reports.TrailBalance
{
    public class TrailBalance
    {
        public List<AccountTypeLevel> AccountTypeLevels { get; set; }
        public decimal DebitSum { get; set; }
        public decimal CreditSum { get; set; }
        public decimal Difference { get; set; }
    }

    public class AccountTypeLevel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? CurrentBalance { get; set; }
        public List<GroupTypeLevel>? GroupTypeLevels { get; set; }
    }

    public class GroupTypeLevel
    {
        public int Id { get; set; }
        public int AccountTypeId { get; set; }
        public string Name { get; set; }
        public string CharkhataNumber { get; set; }
        public decimal? CurrentBalance { get; set; }
        public List<LedgerLevel>? LedgerLevels { get; set; }

    }

    public class LedgerLevel
    {
        public int Id { get; set; }
        public int GroupTypeId { get; set; }
        public string Name { get; set; }
        public decimal? CurrentBalance { get; set; }
    }
}