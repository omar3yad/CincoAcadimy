using CincoAcadimy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CincoAcadimy.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context; // Add this line

        public UserRoleRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context) // Update constructor
        {
            _userManager = userManager;
            _context = context; // Add this line
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<ApplicationUser> GetUserWithStudentAsync(string userName)
        {
            return await _context.Users
                .Include(u => u.Student) // ✅ يجيب Student
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

    }
}
