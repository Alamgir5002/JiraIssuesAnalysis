using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication7.Models
{
    public class CustomField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Custom field key can't be null")]
        public string CustomFieldKey { get; set; }
        [Required (ErrorMessage = "Custom field value can't be null")]
        public string CustomFieldValue { get; set; }
    }
}
