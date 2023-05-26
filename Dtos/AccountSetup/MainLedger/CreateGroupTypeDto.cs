using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.AccountSetup.MainLedger
{
    public class CreateGroupTypeDto
    {
        [Required]
        public string Name { get; set; }
        public string? NepaliName { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime EntryDate { get; set; }

        public int? Schedule { get; set; }
        
        [Required]
        public int AccountTypeId { get; set; }
    }
}