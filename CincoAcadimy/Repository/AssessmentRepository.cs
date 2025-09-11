using CincoAcadimy.Models;
using CincoAcadimy.Models;
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
        }
    }