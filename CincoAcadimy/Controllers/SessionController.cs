using CincoAcadimy.DTOs;
using CincoAcadimy.IServices;
using CincoAcadimy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CincoAcadimy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _service;

        public SessionController(ISessionService service)
        {
            _service = service;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var session = await _service.GetByIdAsync(id);
            if (session == null) return NotFound();
            return Ok(session);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin,Instructor")]

        public async Task<IActionResult> Add(AddSessionDto dto)
        {
            var result = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Update(int id, UpdateSessionDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var result = await _service.UpdateAsync(dto);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }

        [HttpGet("course/{courseId}/student/{studentId}")]

        public async Task<IActionResult> GetSessionsByCourseId(int courseId, int studentId)
        {
            var sessions = await _service.GetSessionsByCourseIdAsync(courseId, studentId);
            return Ok(sessions);
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetSessionsByCourse(int courseId)
        {
            var sessions = await _service.GetSessionsByCourseIdAsync(courseId);

            if (sessions == null || !sessions.Any())
                return NotFound(new { Message = "No sessions found for this course" });

            return Ok(sessions);
        }

        [HttpGet("{sessionId}/student/{studentId}/attendance")]
        //[Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> GetStudentAttendance(int sessionId, int studentId)
        {
            var attendance = await _service.GetStudentSessionAttendanceAsync(sessionId, studentId);
            if (attendance == null)
                return NotFound();

            return Ok(attendance);
        }

        [HttpPut("{sessionId}/student/{studentId}/completion")]
        //[Authorize(Roles = "Instructor")]
        public async Task<IActionResult> UpdateCompletion(int sessionId, int studentId, [FromBody] UpdateCompletionDto dto)
        {
            var success = await _service.UpdateCompletionAsync(sessionId, studentId, dto.IsCompleted);

            if (success)
                return Ok(new { Message = "Completion status updated successfully." });

            return BadRequest(new { Message = "Failed to update or create completion record." });
        }

        [HttpPost("add/Resource")]
        //[Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Add([FromBody] AddResourceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
    }
}