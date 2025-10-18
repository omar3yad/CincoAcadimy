namespace CincoAcadimy.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string ImageUrl { get; set; }
        public string Duration { get; set; }

        public decimal Price { get; set; }

        public int InstructorId { get; set; }
        public Instructor    Instructor { get; set; }

        // Navigation
        public ICollection<StudentCourse> StudentCourses { get; set; }
        public ICollection<Session> Sessions { get; set; }   // 👈 كل كورس له سيشنات
    }
}
