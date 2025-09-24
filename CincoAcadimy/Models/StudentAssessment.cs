namespace CincoAcadimy.Models
{
    public class StudentAssessment
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        // 🔗 لينك الحل على Google Drive
        public string? SubmissionLink { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        public double Grade { get; set; }  // أو Mark لو عايز
        public int AssessmentId { get; set; }
        public Assessment Assessment { get; set; }

    }
}

