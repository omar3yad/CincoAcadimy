using System.ComponentModel.DataAnnotations;

namespace CincoAcadimy.Models
{
    public class Resource
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } // e.g., "Syllabus.pdf" or "Intro Slides"

        [MaxLength(500)]
        public string Description { get; set; } // Short explanation of the resource

        [Required]
        [MaxLength(300)]
        public string Url { get; set; } // Link to file (PDF, PPTX, DOCX, external web link)

        [MaxLength(50)]
        public string FileType { get; set; } // e.g., "PDF", "Slides", "Video", "Link"

        // علاقة كل Resource بجلسة معينة
        [Required]
        public int SessionId { get; set; }
        public Session Session { get; set; }

        // Metadata
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow; // When the resource was added
        public bool IsDownloadable { get; set; } = true; // If true, student can download it
    }
}
