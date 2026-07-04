using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SNRS.Models
{
    public class Report
    {
        [Key]
        public int ReportID { get; set; }

        [Required]
        public int ProjectID { get; set; }

        [ForeignKey("ProjectID")]
        public Project? Project { get; set; }

        [Required]
        public int GeneratedByID { get; set; }

        [ForeignKey("GeneratedByID")]
        public User? GeneratedBy { get; set; }

        [Required, MaxLength(50)]
        public string ReportType { get; set; } = string.Empty; // Progress / Financial / Environmental

        public DateTime ReportDate { get; set; } = DateTime.Now;

        [Required, MaxLength(500)]
        public string Summary { get; set; } = string.Empty;
    }
}