using CincoAcadimy.DTOs;
using CincoAcadimy.Models;
using Microsoft.AspNetCore.Identity;

namespace CincoAcadimy.Repository.@interface
{
    public interface IUserRoleRepository
    {
        Task<ApplicationUser?> GetUserByIdAsync(string userId);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role);
        Task<ApplicationUser> GetUserWithStudentAsync(string userName);

        Task<List<Instructor>> GetAllAsync();
        Task<StudentDashboardDto> GetDashboardDataAsync(int studentId);


    }
}
