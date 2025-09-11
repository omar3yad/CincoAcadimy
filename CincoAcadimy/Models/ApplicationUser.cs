using CincoAcadimy.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CincoAcadimy.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string? FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Student? Student { get; set; }
        public Instructor? Instructor { get; set; }
        public HR? HR { get; set; }
        public Admin? Admin { get; set; }
    }
}
