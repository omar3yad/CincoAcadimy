namespace CincoAcadimy.DTOs
{
    public class StudentAssessmentReadDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int AssessmentId { get; set; }
        public string AssessmentTitle { get; set; }
        public string? SubmissionLink { get; set; }
        public DateTime SubmittedAt { get; set; }
        public double Grade { get; set; }
    }

    //public class StudentAssessmentCreateDto
    //{
    //    public int StudentId { get; set; }
    //    public int AssessmentId { get; set; }
    //    public string? SubmissionLink { get; set; }
    //    public DateTime SubmittedAt { get; set; }
    //    public double Grade { get; set; }
    //}

    //public class StudentAssessmentUpdateDto
    //{
    //    public string? SubmissionLink { get; set; }
    //    public DateTime SubmittedAt { get; set; }
    //    public double Grade { get; set; }
    //}

    //public class StudentAssessmentSubmitDto
    //{
    //    public int StudentId { get; set; }
    //    public int AssessmentId { get; set; }
    //    public string SubmissionLink { get; set; }
    //}

    //public class StudentAssessmentGradeDto
    //{
    //    public double Grade { get; set; }
    //}
}   