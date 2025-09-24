using CincoAcadimy.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CincoAcadimy.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string? Major { get; set; }
        public ICollection<StudentSession> StudentSessions { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<StudentAssessment> StudentAssessments { get; set; } // <-- Add this property
    }

}
