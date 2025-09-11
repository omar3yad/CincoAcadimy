using CincoAcadimy.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CincoAcadimy.Models
{
    public class Instructor
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string Specialization { get; set; }

        // Navigation
        public ICollection<Course> Courses { get; set; }
    }
}