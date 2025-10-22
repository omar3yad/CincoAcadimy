using System.ComponentModel.DataAnnotations;

namespace CincoAcadimy.Models.Authorization
{
    /// <summary>
    /// Represents a permission that can be assigned to roles
    /// </summary>
    public class RolePermission
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Role { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Permission { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
