using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.AccountSetup.MainLedger
{
    public class SubLedgerDto
    {
        public int Id { get; set; }
        public int SubLedgerCode { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int LedgerId { get; set; }
        public string LedgerName { get; set; }
        public string AccountTypeName { get; set; }
        public string GroupTypeName { get; set; }
    }
}