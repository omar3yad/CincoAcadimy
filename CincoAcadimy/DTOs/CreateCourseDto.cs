namespace CincoAcadimy.DTOs
{
    public class CreateCourseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }


        public string ImageUrl { get; set; }

        public string duration { get; set; }

        public decimal Price { get; set; }
        public int InstructorId { get; set; }

    }
}