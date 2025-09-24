using CincoAcadimy.DTOs;
using CincoAcadimy.Models;

namespace CincoAcadimy.Service
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


    }
}
