namespace CincoAcadimy.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        // Instructor Name فقط
        public string InstructorName { get; set; }
    }
}
