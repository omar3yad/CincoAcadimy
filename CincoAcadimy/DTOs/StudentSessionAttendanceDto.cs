namespace CincoAcadimy.DTOs
{
    public class StudentSessionAttendanceDto
    {
        public int StudentId { get; set; }
        public int SessionId { get; set; }
        public bool IsCompleted { get; set; }
    }
}