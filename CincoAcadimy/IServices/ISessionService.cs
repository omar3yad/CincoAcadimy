using CincoAcadimy.DTOs;
using CincoAcadimy.Models;

namespace CincoAcadimy.IServices
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionDto>> GetAllAsync();
        Task<SessionDto> GetByIdAsync(int id);
        Task<SessionDto> AddAsync(AddSessionDto dto);
        Task<SessionDto> UpdateAsync(UpdateSessionDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<SessionDto>> GetSessionsByCourseIdAsync(int courseId, int studentId);

        Task<IEnumerable<SessionDto>> GetSessionsByCourseIdAsync(int courseId);

        Task<bool> UpdateCompletionAsync(int sessionId, int studentId, bool isCompleted);

        Task<Resource> AddAsync(AddResourceDto dto);
        Task<StudentSessionAttendanceDto> GetStudentSessionAttendanceAsync(int sessionId, int studentId);
    }
}
