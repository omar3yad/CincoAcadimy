using CincoAcadimy.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CincoAcadimy.Service.@interface
{
    public interface IPaymentService
    {
        Task<PaymentDto> AddAsync(CreatePaymentDto dto, string screenshotPath);
        Task<PaymentDto?> GetByIdAsync(int id);
        Task<IEnumerable<PaymentDto>> GetAllAsync();
        Task DeleteAsync(int id);
        Task<PaymentDto?> UpdateStatusAsync(int paymentId, string status); // Added

        Task<IEnumerable<PaymentDto>> GetPaymentsByStudentIdAsync(int studentId);
    }
}