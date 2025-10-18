namespace CincoAcadimy.Models
{
    public class StudentCourse
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public bool IsEnrolled { get; set; } = false;
        public double Progress { get; set; } = 0.0; // نسبة م

        public bool IsCompleted { get; set; } = false;
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    }
}
