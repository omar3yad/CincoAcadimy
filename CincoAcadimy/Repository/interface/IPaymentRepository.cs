using CincoAcadimy.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CincoAcadimy.Repository.@interface
{
    public interface IPaymentRepository
    {
        Task<Payment> AddAsync(Payment payment);
        Task<Payment?> GetByIdAsync(int id);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task DeleteAsync(int id);
        Task<Payment?> UpdateStatusAsync(int paymentId, string status); // Added
        Task<IEnumerable<Payment>> GetPaymentsByStudentIdAsync(int studentId);

    }
}