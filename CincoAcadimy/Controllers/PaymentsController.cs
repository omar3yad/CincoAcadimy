using CincoAcadimy.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CincoAcadimy.IServices;

namespace CincoAcadimy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;
        private readonly IWebHostEnvironment _env;

        public PaymentsController(IPaymentService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [HttpPost]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> Create([FromForm] CreatePaymentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "payments");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = Guid.NewGuid() + Path.GetExtension(dto.Screenshot.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Screenshot.CopyToAsync(stream);
            }

            var screenshotPath = $"/uploads/payments/{fileName}";

            var result = await _service.AddAsync(dto, screenshotPath);

            return Ok(result);
        }


        [HttpGet]
        //[Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _service.GetAllAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Get(int id)
        {
            var payment = await _service.GetByIdAsync(id);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("status")]
        //[Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdatePaymentStatusDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateStatusAsync(dto.PaymentId, dto.Status);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        // ✅ New endpoint: Get all payments for a specific student
        [HttpGet("student/{studentId}")]
        //[Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GetPaymentsByStudentId(int studentId)
        {
            var payments = await _service.GetPaymentsByStudentIdAsync(studentId);
            if (!payments.Any())
                return NotFound(new { message = "No payments found for this student." });

            return Ok(payments);
        }
    }
}