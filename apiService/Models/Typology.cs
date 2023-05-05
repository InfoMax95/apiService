using System.ComponentModel.DataAnnotations;

namespace apiService.Models
{
    public class Typology
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
