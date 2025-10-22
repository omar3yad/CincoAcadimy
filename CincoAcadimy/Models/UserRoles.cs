namespace CincoAcadimy.Models
{
    /// <summary>
    /// Defines the available user roles in the system
    /// </summary>
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Instructor = "Instructor";
        public const string HR = "HR";
        public const string Student = "Student";
        
        /// <summary>
        /// Gets all available roles
        /// </summary>
        public static readonly string[] AllRoles = { Admin, Instructor, HR, Student };
        
        /// <summary>
        /// Gets roles that can manage users (Admin and HR)
        /// </summary>
        public static readonly string[] ManagementRoles = { Admin, HR };
        
        /// <summary>
        /// Gets roles that can access instructor functions
        /// </summary>
        public static readonly string[] InstructorRoles = { Admin, Instructor };
        
        /// <summary>
        /// Gets roles that can access student functions
        /// </summary>
        public static readonly string[] StudentRoles = { Admin, Student };
    }
}
