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
                Name = s.Name,
                Description = s.Description,
                VideoUrl = s.VideoUrl,
                Resources = s.Resources.Select(r => new ResourceDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Url = r.Url,
                    FileType = r.FileType,
                    IsDownloadable = r.IsDownloadable
                }).ToList()?? new List<ResourceDto>()
            });
        }

        public async Task<SessionDto> GetByIdAsync(int id)
        {
            var s = await _repository.GetByIdAsync(id);
            if (s == null) return null;

            return new SessionDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                VideoUrl = s.VideoUrl,
                Resources = s.Resources.Select(r => new ResourceDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Url = r.Url,
                    FileType = r.FileType,
                    IsDownloadable = r.IsDownloadable
                }).ToList()
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
                Name = result.Name,
                //CourseId = result.CourseId
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
                Name = result.Name,
                //CourseId = result.CourseId
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
        public async Task<IEnumerable<SessionDto>> GetSessionsByCourseIdAsync(int courseId, int studentId)
        {
            var sessions = await _repository.GetSessionsByCourseIdAsync(courseId, studentId);

            return sessions.Select(s => new SessionDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                VideoUrl = s.VideoUrl,
                IsCompleted = s.StudentSessions
                              .FirstOrDefault(ss => ss.StudentId == studentId)?.IsCompleted ?? false,

                Resources = s.Resources.Select(r => new ResourceDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Url = r.Url,
                    FileType = r.FileType,
                    IsDownloadable = r.IsDownloadable
                }).ToList()?? new List<ResourceDto>()
            });
        }

        public async Task<IEnumerable<SessionDto>> GetSessionsByCourseIdAsync(int courseId)
        {
            var sessions = await _repository.GetSessionsByCourseIdAsync(courseId);

            return sessions.Select(s => new SessionDto
            {
                Id = s.Id,
                Name = s.Name,
            });
        }


    }
}