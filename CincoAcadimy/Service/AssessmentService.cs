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
                SessionId = a.SessionId
            }).ToList();
        }

        public async Task<Assessment?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(AddAssessmentDto dto)
        {
            var assessment = new Assessment
            {
                Title = dto.Title,
                Description = dto.Description,
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
    }
}