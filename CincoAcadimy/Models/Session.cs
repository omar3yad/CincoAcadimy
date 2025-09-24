using System.ComponentModel.DataAnnotations;

namespace CincoAcadimy.Models
{
    public class Session
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }  // Overview or summary of the session


        // كل سيشن تبع كورس
        public int CourseId { get; set; }
        public Course Course { get; set; }
        [Url]
        public string VideoUrl { get; set; }  // Embedded video link (YouTube, Vimeo, or hosted video)
                                                        
        // Initialize lists to avoid null
        public ICollection<Resource> Resources { get; set; } = new List<Resource>();
        public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<StudentSession> StudentSessions { get; set; } = new List<StudentSession>(); public DateTime? StartDate { get; set; } // Optional: when the session goes live

        public DateTime? EndDate { get; set; }   // Optional: deadline or availability window
    }

}
