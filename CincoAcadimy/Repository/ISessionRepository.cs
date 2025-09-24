using CincoAcadimy.Models;

namespace CincoAcadimy.Repository
{
    public interface ISessionRepository
    {
        Task<IEnumerable<Session>> GetAllAsync();
        Task<Session> GetByIdAsync(int id);
        Task<Session> AddAsync(Session session);
        Task<Session> UpdateAsync(Session session);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Session>> GetSessionsByCourseIdAsync(int courseId, int studentId);

        Task<IEnumerable<Session>> GetSessionsByCourseIdAsync(int courseId);


    }
}
