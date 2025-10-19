using CincoAcadimy.Models;
using CincoAcadimy.Repository.@interface;
using Microsoft.EntityFrameworkCore;

namespace CincoAcadimy.Repository
{
    public class SessionRepository : ISessionRepository
    {
        private readonly ApplicationDbContext _context;

        public SessionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Session>> GetAllAsync()
        {
            return await _context.Sessions
                .Include(s => s.Resources)
                .Include(s => s.Course).ToListAsync();
        }

        public async Task<Session> GetByIdAsync(int id)
        {
            return await _context.Sessions
                .Include(s => s.Course)
                .Include(s => s.Resources)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Session> AddAsync(Session session)
        {
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<Session> UpdateAsync(Session session)
        {
            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session == null) return false;

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Session>> GetSessionsByCourseIdAsync(int courseId, int studentId)
        {
            return await _context.Sessions
                .Where(s => s.CourseId == courseId)
                                .Include(s => s.Resources)
                .Include(s => s.StudentSessions) // هنستخدمها عشان نعرف الطالب خلص ولا لأ
                .ToListAsync();
        }

        public async Task<IEnumerable<Session>> GetSessionsByCourseIdAsync(int courseId)
        {
            return await _context.Sessions
                .Where(s => s.CourseId == courseId)
                .Include(s => s.Resources)
                .Include(s => s.StudentSessions) // هنستخدمها عشان نعرف الطالب خلص ولا لأ
                .Include(s => s.Course).ToListAsync();
        }

        // NEW METHOD
        public async Task<StudentSession> GetStudentSessionAttendanceAsync(int sessionId, int studentId)
        {
            return await _context.StudentSessions
                .FirstOrDefaultAsync(ss => ss.SessionId == sessionId && ss.StudentId == studentId);
        }

        public async Task<bool> UpdateCompletionAsync(int sessionId, int studentId, bool isCompleted)
        {
            var record = await _context.StudentSessions
                .FirstOrDefaultAsync(ss => ss.SessionId == sessionId && ss.StudentId == studentId);

            if (record == null)
            {
                // ⬇️ لو السجل مش موجود → أضفه جديد
                record = new StudentSession
                {
                    SessionId = sessionId,
                    StudentId = studentId,
                    IsCompleted = isCompleted
                };

                _context.StudentSessions.Add(record);
            }
            else
            {
                // ⬇️ لو موجود → حدث الحالة
                record.IsCompleted = isCompleted;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Resource> AddAsync(Resource resource)
        {
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();
            return resource;
        }
    }
}
