using CincoAcadimy.Models;
using Microsoft.AspNetCore.Identity;

namespace CincoAcadimy.Repositories
{
    public interface IUserRoleRepository
    {
        Task<ApplicationUser?> GetUserByIdAsync(string userId);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role);
        Task<ApplicationUser> GetUserWithStudentAsync(string userName);
    }
}
