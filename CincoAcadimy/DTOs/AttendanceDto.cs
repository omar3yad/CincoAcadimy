namespace CincoAcadimy.DTOs
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int StudentId { get; set; }
        public bool IsPresent { get; set; }

        // Optional for frontend clarity
        public string StudentName { get; set; }
        public string SessionName { get; set; }
    }
}