using CincoAcadimy.Models;
using global::CincoAcadimy.Repository.@interface;
using Microsoft.EntityFrameworkCore;

namespace CincoAcadimy.Repository
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly ApplicationDbContext _context;

        public ResourceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Resource>> GetAllAsync()
        {
            return await _context.Resources.ToListAsync();
        }

        public async Task<IEnumerable<Resource>> GetBySessionIdAsync(int sessionId)
        {
            return await _context.Resources
                .Where(r => r.SessionId == sessionId)
                .ToListAsync();
        }

        public async Task<Resource?> GetByIdAsync(int id)
        {
            return await _context.Resources.FindAsync(id);
        }

        public async Task<Resource> AddAsync(Resource resource)
        {
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();
            return resource;
        }

        public async Task<bool> UpdateAsync(Resource resource)
        {
            _context.Resources.Update(resource);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var res = await _context.Resources.FindAsync(id);
            if (res == null) return false;

            _context.Resources.Remove(res);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
