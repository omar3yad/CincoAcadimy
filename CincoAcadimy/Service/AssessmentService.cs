using CincoAcadimy.DTOs;
using CincoAcadimy.Models;
using CincoAcadimy.Repository;

namespace CincoAcadimy.Service
{
    public class AssessmentService : IAssessmentService
    {
        private readonly IAssessmentRepository _repository;

        public AssessmentService(IAssessmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AssessmentDto>> GetAllAsync()
        {
            var assessments = await _repository.GetAllAsync();

            return assessments.Select(a => new AssessmentDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                FilePath = a.FilePath,
                SessionId = a.SessionId
            }).ToList();
        }

        public async Task<AssessmentDto?> GetByIdAsync(int id)
        {
            var a = await _repository.GetByIdAsync(id);

            if (a == null) return null;

            var dto = new AssessmentDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                FilePath = a.FilePath,
                SessionId = a.SessionId
            };

            return dto;
        }

        public async Task AddAsync(AddAssessmentDto dto)
        {
            var assessment = new Assessment
            {
                Title = dto.Title,
                Description = dto.Description,
                FilePath = dto.FileType,
                SessionId = dto.SessionId
            };

            await _repository.AddAsync(assessment);
        }

        public async Task UpdateAsync(int id, AddAssessmentDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing != null)
            {
                existing.Title = dto.Title;
                existing.Description = dto.Description;
                existing.SessionId = dto.SessionId;

                await _repository.UpdateAsync(existing);
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task<StudentAssessment> uploadAsync(UploadDto request)
        {
            var entity = new StudentAssessment
            {
                StudentId =request.StudentId,
                AssessmentId = request.AssessmentId,
                SubmittedAt = DateTime.UtcNow,
                Grade = -1, // أو null لو هتخليها Nullable
                SubmissionLink = request.SubmissionLink,
            };

            return await _repository.uploadAsync(entity);
        }

        public async Task<List<AssessmentDto>> GetBySessionIdAsync(int sessionId)
        {
            var assessments = await _repository.GetBySessionIdAsync(sessionId);

            return assessments.Select(a => new AssessmentDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                FilePath = a.FilePath,
                SessionId = a.SessionId
            }).ToList();
        }

    }
}