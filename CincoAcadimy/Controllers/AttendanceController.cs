//using CincoAcadimy.DTOs;
//using CincoAcadimy.Service;
//using Microsoft.AspNetCore.Mvc;

//namespace CincoAcadimy.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AttendanceController : ControllerBase
//    {
//        private readonly IAttendanceService _service;

//        public AttendanceController(IAttendanceService service)
//        {
//            _service = service;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var result = await _service.GetAllAsync();
//            return Ok(result);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var result = await _service.GetByIdAsync(id);
//            if (result == null) return NotFound();
//            return Ok(result);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Add([FromBody] AddAttendanceDto dto)
//        {
//            var result = await _service.AddAsync(dto);
//            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, [FromBody] AddAttendanceDto dto)
//        {
//            var updated = await _service.UpdateAsync(id, dto);
//            if (!updated) return NotFound();
//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var deleted = await _service.DeleteAsync(id);
//            if (!deleted) return NotFound();
//            return NoContent();
//        }
//    }
//}
