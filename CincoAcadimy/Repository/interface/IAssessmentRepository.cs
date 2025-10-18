using CincoAcadimy.Models;

namespace CincoAcadimy.Repository.@interface
{
    public interface IAssessmentRepository
    {
        Task<IEnumerable<Assessment>> GetAllAsync();
        Task<Assessment?> GetByIdAsync(int id);
        Task AddAsync(Assessment assessment);
        Task UpdateAsync(Assessment assessment);
        Task DeleteAsync(int id);
        Task<IEnumerable<StudentAssessment>> GetAllStudentAssessmenAsync();

        Task<StudentAssessment> uploadAsync(StudentAssessment entity);

        Task<List<Assessment>> GetBySessionIdAsync(int sessionId);


        Task UpdateStudentAssessmentAsync(StudentAssessment studentAssessment);
        Task<StudentAssessment?> GetStudentAssessmentAsync(int studentId, int assessmentId);



    }
}