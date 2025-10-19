using CincoAcadimy.DTOs;
using CincoAcadimy.Models;
using global::CincoAcadimy.Repository.@interface;
using System.ComponentModel.Design;
using System.Globalization;
using CincoAcadimy.IServices;

using System.Resources;
using IResourceService = CincoAcadimy.IServices.IResourceService;

namespace CincoAcadimy.Service

{
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepository _repo;

        public ResourceService(IResourceRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Resource>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<IEnumerable<Resource>> GetBySessionIdAsync(int sessionId)
            => await _repo.GetBySessionIdAsync(sessionId);

        public async Task<Resource?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<Resource> AddAsync(AddResourceDto dto)
        {
            var resource = new Resource
            {
                Title = dto.Title,
                Description = dto.Description,
                Url = dto.Url,
                FileType = dto.FileType,
                SessionId = dto.SessionId,
                UploadedAt = DateTime.UtcNow,
                IsDownloadable = dto.IsDownloadable
            };

            return await _repo.AddAsync(resource);
        }

        public async Task<bool> UpdateAsync(int id, ResourceDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.Url = dto.Url;
            existing.FileType = dto.FileType;
            existing.SessionId = dto.SessionId;
            existing.IsDownloadable = dto.IsDownloadable;

            return await _repo.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);

        public IResourceReader? GetResourceReader(CultureInfo info)
        {
            throw new NotImplementedException();
        }

        public IResourceWriter GetResourceWriter(CultureInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
