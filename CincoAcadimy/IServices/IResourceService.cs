using CincoAcadimy.DTOs;
using CincoAcadimy.Models;

namespace CincoAcadimy.IServices
{
    public interface IResourceService
    {
        Task<IEnumerable<Resource>> GetAllAsync();
        Task<IEnumerable<Resource>> GetBySessionIdAsync(int sessionId);
        Task<Resource?> GetByIdAsync(int id);
        Task<Resource> AddAsync(AddResourceDto dto);
        Task<bool> UpdateAsync(int id, ResourceDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
