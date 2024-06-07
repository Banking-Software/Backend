using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFinance.Models.CompanyProfile
{
    public class Calendar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int NumberOfDay { get; set; }
        public int RunningDay { get; set; }=1;
        public bool IsActive { get; set; }
        public bool? IsLocked { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
    }
}