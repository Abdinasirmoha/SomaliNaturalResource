using System.ComponentModel.DataAnnotations;

namespace SNRS.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required, MaxLength(50)]
        public string CategoryName { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        // Navigation
        public ICollection<Resource>? Resources { get; set; }
    }
}