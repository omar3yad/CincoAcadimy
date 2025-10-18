using CincoAcadimy.DTOs;
using CincoAcadimy.Models;
using CincoAcadimy.Service.@interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CincoAcadimy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentsController : ControllerBase
    {
  
        private readonly IAssessmentService _service;

        public AssessmentsController(IAssessmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assessment>>> GetAll()
        {
            var assessments = await _service.GetAllAsync();
            return Ok(assessments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Assessment>> GetById(int id)
        {
            var assessment = await _service.GetByIdAsync(id);
            if (assessment == null) return NotFound();
            return Ok(assessment);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] AddAssessmentDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Title) || dto.SessionId <= 0)
                return BadRequest(new { message = "Invalid assessment data." });

            await _service.AddAsync(dto);
            return Ok(new { message = "Assessment created successfully." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, CincoAcadimy.DTOs.AddAssessmentDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new { message = "Assessment updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(new { message = "Assessment deleted successfully" });
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Create(UploadDto request)
        {
            var result = await _service.uploadAsync(request);
            return Ok(new { message = "Assessment uploaded successfully" });
        }

        [HttpGet("session/{sessionId}")]
        public async Task<IActionResult> GetBySessionId(int sessionId)
        {
            var assessments = await _service.GetBySessionIdAsync(sessionId);

            if (assessments == null || !assessments.Any())
                return NotFound(new { message = "No assessments found for this session." });

            return Ok(assessments);
        }

        [HttpGet("StudentAssessmen")]
        public async Task<ActionResult<IEnumerable<StudentAssessmentReadDto>>> GetAllStudentAssessmenAsync()
        {
            var result = await _service.GetAllStudentAssessmenAsync();
            return Ok(result);
        }

        [HttpPut("update-grade")]
        public async Task<IActionResult> UpdateGrade([FromBody] UpdateGradeDto dto)
        {
            var result = await _service.UpdateGradeAsync(dto.StudentId, dto.AssessmentId, dto.Grade);

            if (!result)
                return NotFound(new { message = "Student assessment not found." });

            return Ok(new { message = "Grade updated successfully." });
        }

    }
}