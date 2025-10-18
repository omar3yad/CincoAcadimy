using CincoAcadimy.DTOs;
using CincoAcadimy.Models;
using CincoAcadimy.Repository;
using CincoAcadimy.Repository.@interface;
using CincoAcadimy.Service.@interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CincoAcadimy.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repo;
        public PaymentService(IPaymentRepository repo)
        {
            _repo = repo;
        }

        public async Task<PaymentDto> AddAsync(CreatePaymentDto dto, string screenshotPath)
        {
            var payment = new Payment
            {
                Phone = dto.Phone,
                ScreenshotPath = screenshotPath,
                CourseId = dto.CourseId,
                StudentId = dto.StudentId,          // 👈 لازم
                Price = dto.CoursePrice,      // 👈 لازم
                CreatedAt = DateTime.UtcNow,
                Status = "Pending"                  // 👈 Default value
            };

            var result = await _repo.AddAsync(payment);

            return new PaymentDto
            {
                Id = result.Id,
                Phone = result.Phone,
                ScreenshotPath = result.ScreenshotPath,
                CourseId = result.CourseId,
                StudentId = result.StudentId,       // 👈 رجعه في الـ DTO
                CoursePrice = result.Price,   // 👈 رجعه في الـ DTO
                CreatedAt = result.CreatedAt,
                Status = result.Status              // 👈 رجعه في الـ DTO
            };
        }

        public async Task<PaymentDto?> GetByIdAsync(int id)
        {
            var payment = await _repo.GetByIdAsync(id);
            if (payment == null) return null;
            return new PaymentDto
            {
                Id = payment.Id,
                Phone = payment.Phone,
                ScreenshotPath = payment.ScreenshotPath,
                CourseId = payment.CourseId,
                CreatedAt = payment.CreatedAt
            };
        }

        public async Task<IEnumerable<PaymentDto>> GetAllAsync()
        {
            var payments = await _repo.GetAllAsync();
            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                Phone = p.Phone,
                ScreenshotPath = p.ScreenshotPath,
                CourseId = p.CourseId,
                StudentId = p.StudentId,
                CoursePrice = p.Price,
                Status = p.Status,
                CreatedAt = p.CreatedAt

            });
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<PaymentDto?> UpdateStatusAsync(int paymentId, string status)
        {
            var updated = await _repo.UpdateStatusAsync(paymentId, status);
            if (updated == null) return null;
            return new PaymentDto
            {
                Id = updated.Id,
                Phone = updated.Phone,
                ScreenshotPath = updated.ScreenshotPath,
                CourseId = updated.CourseId,
                StudentId = updated.StudentId,
                CoursePrice = updated.Price,
                Status = updated.Status,
                CreatedAt = updated.CreatedAt
            };
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByStudentIdAsync(int studentId)
        {
            var payments = await _repo.GetPaymentsByStudentIdAsync(studentId);
            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                Phone = p.Phone,
                ScreenshotPath = p.ScreenshotPath,
                CourseId = p.CourseId,
                StudentId = p.StudentId,
                CoursePrice = p.Price,
                Status = p.Status,
                CreatedAt = p.CreatedAt
            });
        }
    }
}