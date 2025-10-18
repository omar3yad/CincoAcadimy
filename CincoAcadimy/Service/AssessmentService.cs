using CincoAcadimy.DTOs;
using CincoAcadimy.Models;
using CincoAcadimy.Repository.@interface;
using CincoAcadimy.Service.@interface;

namespace CincoAcadimy.Service
{
    public class AssessmentService : IAssessmentService
    {
        private readonly IAssessmentRepository _repository;
        private readonly IWebHostEnvironment _env;

        public AssessmentService(IAssessmentRepository repository, IWebHostEnvironment env)
        {
            _repository = repository;
            _env = env;
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
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            string filePath = null;
            if (dto.File != null && dto.File.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.File.FileName);
                var fullPath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await dto.File.CopyToAsync(stream);
                }

                filePath = "/uploads/" + fileName;
            }

            if (string.IsNullOrEmpty(filePath))
                throw new InvalidOperationException("File upload is required.");

            var assessment = new Assessment
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                FilePath = filePath ?? string.Empty,
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

        public async Task<IEnumerable<StudentAssessmentReadDto>> GetAllStudentAssessmenAsync()
        {
            var entities = await _repository.GetAllStudentAssessmenAsync();
            return entities.Select(e => new StudentAssessmentReadDto
            {
                StudentId = e.StudentId,
                StudentName = e.Student.User.FullName,
                AssessmentTitle = e.Assessment.Title,
                AssessmentId = e.AssessmentId,
                SubmittedAt = e.SubmittedAt,
                Grade = e.Grade,
                SubmissionLink = e.SubmissionLink
            });
        }
        public async Task<bool> UpdateGradeAsync(int studentId, int assessmentId, int grade)
        {
            var studentAssessment = await _repository.GetStudentAssessmentAsync(studentId, assessmentId);

            if (studentAssessment == null)
                return false;

            studentAssessment.Grade = grade;
            await _repository.UpdateStudentAssessmentAsync(studentAssessment);
            return true;
        }

    }
}