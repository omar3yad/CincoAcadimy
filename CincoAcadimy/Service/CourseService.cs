using CincoAcadimy.DTOs;
using CincoAcadimy.Models;
using CincoAcadimy.Repository;

namespace CincoAcadimy.Service

{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repository;

        public CourseService(ICourseRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            var courses = await _repository.GetAllAsync();

            // تحويل لـ DTO
            return courses.Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                InstructorName = c.Instructor?.User?.FullName ?? "Unknown"
            });
        }


        public async Task<CourseDto?> GetCourseByIdAsync(int id)
        {
            var course = await _repository.GetByIdAsync(id);
            if (course == null) return null;

            return new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                InstructorName = course.Instructor?.User?.FullName ?? "Unknown"
            };
        }


        public async Task AddCourseAsync(CreateCourseDto dto)
        {
            var course = new Course
            {
                Title = dto.Title,
                Description = dto.Description,
                InstructorId = dto.InstructorId
            };

            await _repository.AddAsync(course);
        }
        public async Task UpdateCourseAsync(Course course)
        {
            await _repository.UpdateAsync(course);
        }

        public async Task DeleteCourseAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
