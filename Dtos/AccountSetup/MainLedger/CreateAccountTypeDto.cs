using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.AccountSetup.MainLedger
{
    public class CreateAccountTypeDto
    {
        [Required]
        public string Name { get; set; }
    }
}