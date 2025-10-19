using CincoAcadimy.DTOs;
using CincoAcadimy.Models;

namespace CincoAcadimy.Repository.@interface
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task AddAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(int id);
        Task<IEnumerable<Course>> GetOngoingCoursesAsync(int studentId);
        Task<IEnumerable<StudentDto>> GetStudentsByCourseIdAsync(int courseId);
        Task<(int studentsCount, int sessionsCount)> GetCountsAsync(int courseId);
        //Task<IEnumerable<StudentDto>> GetStudentsByCourseIdAsync(int courseId)

        Task<IEnumerable<AllCourseDto>> GetAllCoursesForStudentAsync(int studentId);
        Task<AllCourseDto?> GetCourseForStudentAsync(int courseId, int studentId);

        Task<StudentCourse> GetStudentCourseAsync(int studentId, int courseId);
        Task<bool> UpdateEnrollmentStatusAsync(StudentCourse studentCourse);
        Task<bool> SaveChangesAsync();

        Task<IEnumerable<Course>> GetInstructorCoursesAsync(int instructorId);


    }
}
