using CincoAcadimy.DTOs;
using CincoAcadimy.Models;
using CincoAcadimy.Repository;

namespace CincoAcadimy.Service
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _repository;

        public SessionService(ISessionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SessionDto>> GetAllAsync()
        {
            var sessions = await _repository.GetAllAsync();
            return sessions.Select(s => new SessionDto
            {
                Id = s.Id,
                Title = s.Name,
                CourseId = s.CourseId
            });
        }

        public async Task<SessionDto> GetByIdAsync(int id)
        {
            var s = await _repository.GetByIdAsync(id);
            if (s == null) return null;

            return new SessionDto
            {
                Id = s.Id,
                Title = s.Name,
                CourseId = s.CourseId
            };
        }

        public async Task<SessionDto> AddAsync(AddSessionDto dto)
        {
            var session = new Session
            {
                Name = dto.Title,
                CourseId = dto.CourseId
            };

            var result = await _repository.AddAsync(session);

            return new SessionDto
            {
                Id = result.Id,
                Title = result.Name,
                CourseId = result.CourseId
            };
        }

        public async Task<SessionDto> UpdateAsync(UpdateSessionDto dto)
        {
            var existing = await _repository.GetByIdAsync(dto.Id);
            if (existing == null) return null;

            existing.Name = dto.Title;

            var result = await _repository.UpdateAsync(existing);

            return new SessionDto
            {
                Id = result.Id,
                Title = result.Name,
                CourseId = result.CourseId
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}