namespace CincoAcadimy.DTOs
{
    public class CourseReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string InstructorName { get; set; }

        //public ICollection<SessionReadDto> Sessions { get; set; }
    }
}
