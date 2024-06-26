using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.AccountSetup.MainLedger
{
    public class GroupTypeDto
    {
        [Required]
        public int Id { get; set; }
        public int AccountTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        public string NepaliName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EntryDate { get; set; }

        public int? Schedule { get; set; }
        [Required]
        public string CharKhataNumber { get; set; }
        // public string DebitOrCredit { get; set; }
    }
}