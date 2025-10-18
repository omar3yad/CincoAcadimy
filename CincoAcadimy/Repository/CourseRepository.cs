using CincoAcadimy.DTOs;
using CincoAcadimy.Models;
using Microsoft.EntityFrameworkCore;

namespace CincoAcadimy.Repository.@interface
{
    public class CourseRepository: ICourseRepository
    {
        private readonly ApplicationDbContext _context;
        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses
                                 .Include(c => c.Instructor)
                                                         .ThenInclude(i => i.User)

                                 .ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses
                                 .Include(c => c.Instructor)
                                 .ThenInclude(i => i.User)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Course>> GetOngoingCoursesAsync(int studentId)
        {
            return await _context.StudentCourses
                .Where(sc => sc.StudentId == studentId && sc.IsEnrolled == true)
                .Include(sc => sc.Course)
                    .ThenInclude(c => c.Sessions)
                        .ThenInclude(s => s.StudentSessions) // هنا ضم كل الـ StudentSessions
                .Include(sc => sc.Course)
                    .ThenInclude(c => c.Instructor)
                        .ThenInclude(i => i.User)
                .Select(sc => sc.Course)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentDto>> GetStudentsByCourseIdAsync(int courseId)
        {
            return await _context.StudentCourses
                .Where(sc => sc.CourseId == courseId && sc.IsEnrolled)
                .Include(sc => sc.Student)
                .Select(sc => new StudentDto
                {
                    Id = sc.Student.Id,
                    FullName = sc.Student.User.FullName,
                    Email = sc.Student.User.PhoneNumber
                })
                .ToListAsync();
        }

        public async Task<(int studentsCount, int sessionsCount)> GetCountsAsync(int courseId)
        {
            var studentsCount = await _context.StudentCourses.CountAsync(sc => sc.CourseId == courseId);
            var sessionsCount = await _context.Sessions.CountAsync(s => s.CourseId == courseId);
            return (studentsCount, sessionsCount);
        }

        public async Task<IEnumerable<AllCourseDto>> GetAllCoursesForStudentAsync(int studentId)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Select(c => new AllCourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    InstructorName = c.Instructor.User.UserName,
                    ImageUrl = c.ImageUrl,
                    Duration = c.Duration,
                    Price = c.Price,
                    IsEnrolled = c.StudentCourses.Any(sc => sc.StudentId == studentId),
                    Progress = c.StudentCourses
                        .Where(sc => sc.StudentId == studentId)
                        .Select(sc => sc.Progress)
                        .FirstOrDefault(),
                    IsCompleted = c.StudentCourses
                        .Any(sc => sc.StudentId == studentId && sc.IsCompleted)
                })
                .ToListAsync();
        }

        public async Task<AllCourseDto?> GetCourseForStudentAsync(int courseId, int studentId)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Where(c => c.Id == courseId)
                .Select(c => new AllCourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    InstructorName = c.Instructor.User.UserName,
                    ImageUrl = c.ImageUrl,
                    Duration = c.Duration,
                    IsEnrolled = c.StudentCourses.Any(sc => sc.StudentId == studentId),
                    Progress = c.StudentCourses
                        .Where(sc => sc.StudentId == studentId)
                        .Select(sc => sc.Progress)
                        .FirstOrDefault(),
                    IsCompleted = c.StudentCourses
                        .Any(sc => sc.StudentId == studentId && sc.IsCompleted)
                })
                .FirstOrDefaultAsync();
        }

        public async Task<StudentCourse> GetStudentCourseAsync(int studentId, int courseId)
        {
            return await _context.StudentCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);
        }

        public async Task<bool> UpdateEnrollmentStatusAsync(StudentCourse studentCourse)
        {
            _context.StudentCourses.Update(studentCourse);
            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}