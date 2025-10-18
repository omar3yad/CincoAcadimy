using CincoAcadimy.Models;
using CincoAcadimy.Repository.@interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CincoAcadimy.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> AddAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments.FindAsync(id);
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            var query = _context.Payments.AsQueryable();

            return await query.ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Payment?> UpdateStatusAsync(int paymentId, string status)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null) return null;

            payment.Status = status;
            await _context.SaveChangesAsync();

            // ????? ?? ???? StudentCourses ??? ?????? ???????
            var existingEnrollment = await _context.StudentCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == payment.StudentId && sc.CourseId == payment.CourseId);

            if (status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
            {
                if (existingEnrollment != null)
                {
                    existingEnrollment.IsEnrolled = true;
                }
                else
                {
                    var newEnrollment = new StudentCourse
                    {
                        StudentId = payment.StudentId,
                        CourseId = payment.CourseId,
                        EnrollmentDate = DateTime.Now,
                        IsEnrolled = true,
                        IsCompleted = false,
                        Progress = 0
                    };
                    await _context.StudentCourses.AddAsync(newEnrollment);
                }
            }
            else if (status.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
            {
                if (existingEnrollment != null)
                {
                    existingEnrollment.IsEnrolled = false;
                }
            }

            await _context.SaveChangesAsync();

            return payment;
        }
        public async Task<IEnumerable<Payment>> GetPaymentsByStudentIdAsync(int studentId)
        {
            return await _context.Payments
                .Where(p => p.StudentId == studentId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}