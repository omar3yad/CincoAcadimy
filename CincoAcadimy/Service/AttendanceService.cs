using CincoAcadimy.DTOs;
using CincoAcadimy.Models;
using CincoAcadimy.Repository;
using CincoAcadimy.Service;

namespace CincoAcadimy.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _repository;

        public AttendanceService(IAttendanceRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AttendanceDto>> GetAllAsync()
        {
            var attendances = await _repository.GetAllAsync();
            return attendances.Select(a => new AttendanceDto
            {
                Id = a.Id,
                SessionId = a.SessionId,
                StudentId = a.StudentId,
                IsPresent = a.IsPresent,
                StudentName = a.Student?.User.FullName,
                SessionName = a.Session?.Name
            });
        }

        public async Task<AttendanceDto?> GetByIdAsync(int id)
        {
            var attendance = await _repository.GetByIdAsync(id);
            if (attendance == null) return null;

            return new AttendanceDto
            {
                Id = attendance.Id,
                SessionId = attendance.SessionId,
                StudentId = attendance.StudentId,
                IsPresent = attendance.IsPresent,
                StudentName = attendance.Student?.User.FullName,
                SessionName = attendance.Session?.Name
            };
        }

        public async Task<AttendanceDto> AddAsync(AddAttendanceDto dto)
        {
            var attendance = new Attendance
            {
                SessionId = dto.SessionId,
                StudentId = dto.StudentId,
                IsPresent = dto.IsPresent
            };

            await _repository.AddAsync(attendance);
            await _repository.SaveChangesAsync();

            return new AttendanceDto
            {
                Id = attendance.Id,
                SessionId = attendance.SessionId,
                StudentId = attendance.StudentId,
                IsPresent = attendance.IsPresent
            };
        }

        public async Task<bool> UpdateAsync(int id, AddAttendanceDto dto)
        {
            var attendance = await _repository.GetByIdAsync(id);
            if (attendance == null) return false;

            attendance.SessionId = dto.SessionId;
            attendance.StudentId = dto.StudentId;
            attendance.IsPresent = dto.IsPresent;

            await _repository.UpdateAsync(attendance);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var attendance = await _repository.GetByIdAsync(id);
            if (attendance == null) return false;

            await _repository.DeleteAsync(attendance);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
