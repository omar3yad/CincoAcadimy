namespace CincoAcadimy.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }            // من IdentityUser بيكون string GUID
        public string Name { get; set; }          // ممكن تبقى UserName أو FullName لو موجودة
        public string Email { get; set; }
        public string? CompanyName { get; set; }  // لو HR
        public string Role { get; set; }          // Role المستخدم (Student, HR, Admin)
    }
}
