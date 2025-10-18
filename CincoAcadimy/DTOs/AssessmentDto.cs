namespace CincoAcadimy.DTOs
{
    public class AssessmentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? FilePath { get; set; } // Changed from IFormFile? to string?
        public int SessionId { get; set; }
    }
}

