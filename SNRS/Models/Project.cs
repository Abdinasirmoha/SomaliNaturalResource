using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SNRS.Models
{
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }

        [Required, MaxLength(100)]
        public string ProjectName { get; set; } = string.Empty;

        [Required]
        public int ResourceID { get; set; }

        [ForeignKey("ResourceID")]
        public Resource? Resource { get; set; }

        [Required, MaxLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Ongoing"; // Ongoing / Completed / Suspended

        [MaxLength(255)]
        public string? Description { get; set; }

        // Navigation
        public ICollection<Report>? Reports { get; set; }
    }
}