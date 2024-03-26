using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication7.Models
{
    public class SourceCredentials
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Source URL can't be null")]
        public string SourceURL { get; set; }

        [Required(ErrorMessage = "Source User Email can't be null")]
        [EmailAddress(ErrorMessage = "Source User Email address is invalid")]
        public string SourceUserEmail { get; set; }

        [Required(ErrorMessage = "Source Auth Token can't be null")]
        public string SourceAuthToken { get; set; } 
    }
}
