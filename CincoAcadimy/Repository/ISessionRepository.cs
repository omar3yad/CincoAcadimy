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
    }
}
