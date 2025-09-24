using CincoAcadimy.DTOs;
using CincoAcadimy.Models;

namespace CincoAcadimy.Service
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


    }
}