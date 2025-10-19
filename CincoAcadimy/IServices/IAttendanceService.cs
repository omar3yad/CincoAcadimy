using CincoAcadimy.DTOs;

namespace CincoAcadimy.IServices
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceDto>> GetAllAsync();
        Task<AttendanceDto?> GetByIdAsync(int id);
        Task<AttendanceDto> AddAsync(AddAttendanceDto dto);
        Task<bool> UpdateAsync(int id, AddAttendanceDto dto);
        Task<bool> DeleteAsync(int id);
    }
}