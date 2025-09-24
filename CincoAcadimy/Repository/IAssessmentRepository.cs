using CincoAcadimy.Models;

namespace CincoAcadimy.Repository
{
    public interface IAssessmentRepository
    {
        Task<IEnumerable<Assessment>> GetAllAsync();
        Task<Assessment?> GetByIdAsync(int id);
        Task AddAsync(Assessment assessment);
        Task UpdateAsync(Assessment assessment);
        Task DeleteAsync(int id);

        Task<StudentAssessment> uploadAsync(StudentAssessment entity);

        Task<List<Assessment>> GetBySessionIdAsync(int sessionId);



    }
}