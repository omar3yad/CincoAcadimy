using CincoAcadimy.Models;

namespace CincoAcadimy.Models
{
    public class Admin
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string RoleLevel { get; set; } // e.g. SuperAdmin, Manager
    }
}
