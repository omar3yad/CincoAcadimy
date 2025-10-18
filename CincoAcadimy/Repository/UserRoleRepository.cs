using CincoAcadimy.DTOs;
using CincoAcadimy.Models;
using CincoAcadimy.Repository.@interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CincoAcadimy.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

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

        public async Task<List<Instructor>> GetAllAsync()
        {
            return await _context.Instructors
                .Include(i => i.User)
                .Include(i => i.Courses)
                .ToListAsync();
        }

        public async Task<StudentDashboardDto> GetDashboardDataAsync(int studentId)
        {
            // Active courses where the student is enrolled
            var activeCourses = await _context.StudentCourses
                .CountAsync(sc => sc.StudentId == studentId && sc.IsEnrolled && !sc.IsCompleted);

            var completedLessons = await _context.StudentSessions
                .Where(ss => ss.StudentId == studentId && ss.IsCompleted)
                .CountAsync();

            // Overall progress (average of progress column)
            var progressList = await _context.StudentCourses
                .Where(sc => sc.StudentId == studentId)
                .Select(sc => sc.Progress)
                .ToListAsync();

            var overallProgress = progressList.Count > 0 ? (int)progressList.Average() : 0;

            return new StudentDashboardDto
            {
                Message = "You're making great progress in your learning journey. Keep it up!",
                OverallProgress = overallProgress,
                ActiveCourses = activeCourses,
                CompletedLessons = completedLessons
            };
        }

    }
}
