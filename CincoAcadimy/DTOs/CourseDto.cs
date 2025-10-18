namespace CincoAcadimy.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string ImageUrl { get; set; }
        public string Duration { get; set; }

        public decimal Price { get; set; }
        public string InstructorName { get; set; }
    }
}
