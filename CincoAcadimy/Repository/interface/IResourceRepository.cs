using CincoAcadimy.Models;
namespace CincoAcadimy.Repository.@interface
{
    public interface IResourceRepository
    {
        Task<IEnumerable<Resource>> GetAllAsync();
        Task<IEnumerable<Resource>> GetBySessionIdAsync(int sessionId);
        Task<Resource?> GetByIdAsync(int id);
        Task<Resource> AddAsync(Resource resource);
        Task<bool> UpdateAsync(Resource resource);
        Task<bool> DeleteAsync(int id);
    }
}
