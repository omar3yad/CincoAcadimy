using CincoAcadimy.DTOs;
using CincoAcadimy.Models;
using CincoAcadimy.Repository.@interface;
using CincoAcadimy.Service.@interface;

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
                Duration = c.Duration,
                ImageUrl = c.ImageUrl,
                Price = c.Price,
                InstructorName = c.Instructor != null ? c.Instructor.User.UserName : "Unknown"

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
                Duration = course.Duration,
                ImageUrl = course.ImageUrl,
                Price = course.Price,
                InstructorName = course.Instructor?.User?.FullName ?? "Unknown"
            };
        }

        public async Task AddCourseAsync(CreateCourseDto dto)
        {
            var course = new Course
            {
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                Duration = dto.duration,
                Price = dto.Price,
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
                    Description = c.Description,
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

        public async Task<IEnumerable<StudentDto>> GetStudentsByCourseIdAsync(int courseId)
        {
            return await _repository.GetStudentsByCourseIdAsync(courseId);
        }

        public async Task<CourseCountsDto> GetCourseCountsAsync(int courseId)
        {
            var (studentsCount, sessionsCount) = await _repository.GetCountsAsync(courseId);
            return new CourseCountsDto
            {
                StudentsCount = studentsCount,
                SessionsCount = sessionsCount
            };
        }

        public Task<IEnumerable<AllCourseDto>> GetAllCoursesForStudentAsync(int studentId)
        {
            return _repository.GetAllCoursesForStudentAsync(studentId);
        }

        public Task<AllCourseDto?> GetCourseForStudentAsync(int courseId, int studentId)
        {
            return _repository.GetCourseForStudentAsync(courseId, studentId);
        }
        public async Task<bool> UpdateEnrollmentStatusAsync(UpdateEnrollmentStatusDto dto)
        {
            var studentCourse = await _repository.GetStudentCourseAsync(dto.StudentId, dto.CourseId);
            if (studentCourse == null)
                return false;

            studentCourse.IsEnrolled = dto.IsEnrolled;
            return await _repository.UpdateEnrollmentStatusAsync(studentCourse);
        }
    }
}
