using CincoAcadimy.DTOs;
using CincoAcadimy.Models;

namespace CincoAcadimy.Service.@interface
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        Task<CourseDto?> GetCourseByIdAsync(int id);
        Task AddCourseAsync(CreateCourseDto dto);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(int id);

        Task<IEnumerable<OngoingCourseDto>> GetOngoingCoursesAsync(int studentId);
        int CalculateProgress(Course course, int studentId);
        Task<IEnumerable<StudentDto>> GetStudentsByCourseIdAsync(int courseId);
        Task<CourseCountsDto> GetCourseCountsAsync(int courseId);

        Task<IEnumerable<AllCourseDto>> GetAllCoursesForStudentAsync(int studentId);
        Task<AllCourseDto?> GetCourseForStudentAsync(int courseId, int studentId);
        Task<bool> UpdateEnrollmentStatusAsync(UpdateEnrollmentStatusDto dto);
    }
}