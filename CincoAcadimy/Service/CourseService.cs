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
        public async Task<IEnumerable<OngoingCourseDto>> GetOngoingCoursesAsync(int studentId)
        {
            var courses = await _repository.GetOngoingCoursesAsync(studentId);

            var result = courses.Select(c =>
            {
                var sessions = c.Sessions.OrderBy(s => s.Id).ToList();

                var nextLesson = sessions.FirstOrDefault(s =>
                    !s.StudentSessions.Any(ss => ss.StudentId == studentId && ss.IsCompleted)
                );

                return new OngoingCourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    InstructorName = c.Instructor != null ? c.Instructor.User.UserName : "Unknown",
                    Progress = CalculateProgress(c, studentId),
                    NextLesson = nextLesson != null ? new SessionDto
                    {
                        Id = nextLesson.Id,
                        Name = nextLesson.Name,
                        IsCompleted = nextLesson.StudentSessions
                                      .FirstOrDefault(ss => ss.StudentId == studentId)?.IsCompleted ?? false
                    } : null
                };
            });

            return result;
        }

        public int CalculateProgress(Course course, int studentId)
        {
            var sessions = course.Sessions.ToList();
            if (!sessions.Any()) return 0;

            var completed = sessions.Count(s => s.StudentSessions.FirstOrDefault()?.IsCompleted == true);
            return (int)((double)completed / sessions.Count * 100);
        }
    }
}
