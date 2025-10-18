using CincoAcadimy.Models;
using CincoAcadimy.Repository.@interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace CincoAcadimy.Repository
{
        public class AssessmentRepository : IAssessmentRepository
        {
            private readonly ApplicationDbContext _context;

            public AssessmentRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Assessment>> GetAllAsync()
            {
                return await _context.Assessments
                    .Include(a => a.Session)
                    .ToListAsync();
            }

            public async Task<Assessment?> GetByIdAsync(int id)
            {
                return await _context.Assessments
                    .Include(a => a.Session)
                    .FirstOrDefaultAsync(a => a.Id == id);
            }


        public async Task AddAsync(Assessment assessment)
        {
            if (assessment == null) return;
            await _context.Assessments.AddAsync(assessment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Assessment assessment)
            {
                _context.Assessments.Update(assessment);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                var assessment = await _context.Assessments.FindAsync(id);
                if (assessment != null)
                {
                    _context.Assessments.Remove(assessment);
                    await _context.SaveChangesAsync();
                }
            }

        public async Task<StudentAssessment> uploadAsync(StudentAssessment entity)
        {
            _context.StudentAssessments.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Assessment>> GetBySessionIdAsync(int sessionId)
        {
            return await _context.Assessments
                .Where(a => a.SessionId == sessionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentAssessment>> GetAllStudentAssessmenAsync()
        {
            return await _context.StudentAssessments
                .Include(sa => sa.Student)
                            .ThenInclude(s => s.User) // لو Student مرتبط بـ User
                .Include(sa => sa.Assessment)
                .ToListAsync();
        }
        public async Task<StudentAssessment?> GetStudentAssessmentAsync(int studentId, int assessmentId)
        {
            return await _context.StudentAssessments
                .FirstOrDefaultAsync(sa => sa.StudentId == studentId && sa.AssessmentId == assessmentId);
        }

        public async Task UpdateStudentAssessmentAsync(StudentAssessment studentAssessment)
        {
            _context.StudentAssessments.Update(studentAssessment);
            await _context.SaveChangesAsync();
        }


    }
}