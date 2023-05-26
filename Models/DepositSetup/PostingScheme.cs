using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFinance.Models.DepositSetup
{
    public class PostingScheme
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<DepositScheme> DepositScheme { get; set; }
    }
}