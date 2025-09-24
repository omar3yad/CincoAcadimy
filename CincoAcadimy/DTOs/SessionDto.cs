namespace CincoAcadimy.DTOs
{
    public class SessionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }       // Session Title
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public bool IsCompleted { get; set; }

        public List<ResourceDto> Resources { get; set; }
    }
}
