namespace CincoAcadimy.DTOs
{
    public class AddAttendanceDto
    {
        public int SessionId { get; set; }
        public int StudentId { get; set; }
        public bool IsPresent { get; set; }
    }
}
