namespace CincoAcadimy.DTOs
{
    public class OngoingCourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string InstructorName { get; set; }
        public int Progress { get; set; } // percentage
        public SessionDto NextLesson { get; set; }
    }
}
