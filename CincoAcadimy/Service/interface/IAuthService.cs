using CincoAcadimy.DTOs;

namespace CincoAcadimy.Service.@interface
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto model);
        Task<AuthResponseDTO> LoginAsync(LoginDto model);
        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role);
        Task<string> ChangeUserRoleAsync(string userId, string newRole);

        Task<List<InstructorDto>> GetAllInstructorsAsync();

        Task<StudentDashboardDto> GetDashboardAsync(int studentId);

    }
}
