using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFinance.Models.RecordsWithCode
{
    public class RecordsBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Code { get; set; }
        public string? NepaliName { get; set; }
        public string? EnglishName { get; set; }
    }
}