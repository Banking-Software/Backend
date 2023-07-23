using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFinance.Models.ClientSetup
{
    public class ClientBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? ClientId { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public string BranchCode { get; set; }
        [Required]
        public string CreatorId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
        public bool? IsModified { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifierId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ModifiedOn { get; set; }
        public int? ModificationCount { get; set; }
        
    }
}