using System.ComponentModel.DataAnnotations;

namespace CincoAcadimy.Models
{
    public class Session
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // كل سيشن تبع كورس
        public int CourseId { get; set; }
        public Course Course { get; set; }

        // Navigation
        public ICollection<Assessment> Assessments { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
    }
}
