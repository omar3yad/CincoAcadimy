namespace CincoAcadimy.Models
{
    public class StudentAssessment
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int AssessmentId { get; set; }
        public Assessment Assessment { get; set; }

        public double Grade { get; set; }  // أو Mark لو عايز
        public DateTime SubmittedAt { get; set; }
    }
}

