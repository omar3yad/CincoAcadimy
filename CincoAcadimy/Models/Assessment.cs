using System.ComponentModel.DataAnnotations;

namespace CincoAcadimy.Models
{
    public class Assessment
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }   // مثال: Quiz 1, Assignment 2
        public string? Description { get; set; }

        public DateTime DueDate { get; set; }

        // كل Assessment تبع سيشن
        public int SessionId { get; set; }
        public Session Session { get; set; }

        public ICollection<StudentAssessment> StudentAssessments { get; set; }

    }
}
