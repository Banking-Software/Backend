using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.AccountSetup.MainLedger
{
    public class AccountTypeDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}