using CincoAcadimy.DTOs;
using CincoAcadimy.Models;

namespace CincoAcadimy.IServices
{
    public interface IAssessmentService
    {
        Task<IEnumerable<AssessmentDto>> GetAllAsync();
        Task<AssessmentDto?> GetByIdAsync(int id);
        Task AddAsync(AddAssessmentDto dto);

        Task UpdateAsync(int id, AddAssessmentDto dto);
        Task DeleteAsync(int id);
        Task<StudentAssessment> uploadAsync(UploadDto request);

        Task<List<AssessmentDto>> GetBySessionIdAsync(int sessionId);
        Task<IEnumerable<StudentAssessmentReadDto>> GetAllStudentAssessmenAsync();
        Task<bool> UpdateGradeAsync(int studentId, int assessmentId, int grade);




    }
}