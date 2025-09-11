using CincoAcadimy.DTOs;
using CincoAcadimy.Models;
using CincoAcadimy.Service;
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

        [HttpPost]
        public async Task<ActionResult> Add(AddAssessmentDto dto)
        {
            await _service.AddAsync(dto);
            return Ok(new { message = "Assessment created successfully" });
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
    }
}