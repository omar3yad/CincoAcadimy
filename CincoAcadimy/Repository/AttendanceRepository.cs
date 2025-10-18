using CincoAcadimy.Models;
using CincoAcadimy.Repository.@interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace CincoAcadimy.Repository
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ApplicationDbContext _context;

    public AttendanceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Attendance>> GetAllAsync()
    {
        return await _context.Attendances
            .Include(a => a.Student)
            .Include(a => a.Session)
            .ToListAsync();
    }

    public async Task<Attendance?> GetByIdAsync(int id)
    {
        return await _context.Attendances
            .Include(a => a.Student)
            .Include(a => a.Session)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Attendance attendance)
    {
        await _context.Attendances.AddAsync(attendance);
    }

    public async Task UpdateAsync(Attendance attendance)
    {
        _context.Attendances.Update(attendance);
    }

    public async Task DeleteAsync(Attendance attendance)
    {
        _context.Attendances.Remove(attendance);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
}
