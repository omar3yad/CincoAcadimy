namespace CincoAcadimy.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Role { get; set; }
        public int StudentId { get; set; }   // 👈 أضف ده
        public string StudentName { get; set; } // 👈 أضف ده

    }
}
