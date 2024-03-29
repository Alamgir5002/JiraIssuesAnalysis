using System.ComponentModel.DataAnnotations;

namespace WebApplication7.Models
{
    public class Project
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
    }
}
