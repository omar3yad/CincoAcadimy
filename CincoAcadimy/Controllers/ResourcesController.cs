using CincoAcadimy.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CincoAcadimy.IServices;
using CincoAcadimy.Models;

namespace CincoAcadimy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {



        private readonly IResourceService _service;

        public ResourcesController(IResourceService service)
        {
            _service = service;
        }

        // GET: api/Resources
        [HttpGet]
        //[Authorize(Roles = "Admin,Instructor,HR")]
        public async Task<IActionResult> GetAll()
        {
            var resources = await _service.GetAllAsync();
            return Ok(resources);
        }

        // GET: api/Resources/session/{sessionId}
        [HttpGet("session/{sessionId}")]
        //[Authorize(Roles = "Admin,Instructor,HR,Student")]
        public async Task<IActionResult> GetBySession(int sessionId)
        {
            var resources = await _service.GetBySessionIdAsync(sessionId);
            return Ok(resources);
        }

        // GET: api/Resources/{id}
        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin,Instructor,HR,Student")]
        public async Task<IActionResult> GetById(int id)
        {
            var resource = await _service.GetByIdAsync(id);
            if (resource == null)
                return NotFound(new { Message = "Resource not found" });

            return Ok(resource);
        }

        // POST: api/Resources
        [HttpPost]
        //[Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Add([FromBody] AddResourceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }     

        // PUT: api/Resources/{id}
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Update(int id, [FromBody] ResourceDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { Message = "Resource not found" });

            return Ok(new { Message = "Resource updated successfully" });
        }

        // DELETE: api/Resources/{id}
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = "Resource not found" });

            return Ok(new { Message = "Resource deleted successfully" });
        }
    }
}
