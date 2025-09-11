namespace CincoAcadimy.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        // السيشن
        public int SessionId { get; set; }
        public Session Session { get; set; }

        // الطالب
        public int StudentId { get; set; }
        public Student Student { get; set; }

        // حاضر ولا غايب
        public bool IsPresent { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
