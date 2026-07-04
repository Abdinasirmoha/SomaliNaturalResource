using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SNRS.Models
{
    public class Resource
    {
        [Key]
        public int ResourceID { get; set; }

        [Required, MaxLength(100)]
        public string ResourceName { get; set; } = string.Empty;

        [Required]
        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public Category? Category { get; set; }

        [Required, MaxLength(100)]
        public string Location { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; } = 0;

        [Required, MaxLength(20)]
        public string Unit { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Available"; // Available / Depleted / Reserved

        public DateTime DateAdded { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<Project>? Projects { get; set; }
    }
}