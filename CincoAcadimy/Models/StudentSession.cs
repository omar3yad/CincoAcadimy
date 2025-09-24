namespace CincoAcadimy.Models
{
    public class StudentSession
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int SessionId { get; set; }
        public Session Session { get; set; }

        public bool IsCompleted { get; set; } = false; // false by default
    }
}
