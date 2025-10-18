using CincoAcadimy.DTOs;
using CincoAcadimy.Models;
using CincoAcadimy.Service.@interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CincoAcadimy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _service;

        public CourseController(ICourseService service)
        {
            _service = service;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _service.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _service.GetCourseByIdAsync(id);
            if (course == null)
                return NotFound();
            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.AddCourseAsync(dto);
            return Ok("Course created successfully");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Course course)
        {
            if (id != course.Id)
                return BadRequest();

            await _service.UpdateCourseAsync(course);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteCourseAsync(id);
            return NoContent();
        }

        [HttpGet("ongoing/{studentId}")]
        public async Task<IActionResult> GetOngoingCourses(int studentId)
        {
            IEnumerable<OngoingCourseDto> courses = await _service.GetOngoingCoursesAsync(studentId);
            return Ok(courses);
        }

        [HttpGet("{courseId}/students")]
        public async Task<IActionResult> GetStudentsByCourseId(int courseId)
        {
            var students = await _service.GetStudentsByCourseIdAsync(courseId);
            return Ok(students);
        }

        [HttpGet("{id}/counts")]
        public async Task<IActionResult> GetCourseCounts(int id)
        {
            var counts = await _service.GetCourseCountsAsync(id);
            return Ok(counts);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetAllCoursesForStudent(int studentId)
        {
            var courses = await _service.GetAllCoursesForStudentAsync(studentId);
            return Ok(courses);
        }

        [HttpGet("{courseId}/student/{studentId}")]
        public async Task<IActionResult> GetCourseForStudent(int courseId, int studentId)
        {
            var course = await _service.GetCourseForStudentAsync(courseId, studentId);
            if (course == null)
                return NotFound();
            return Ok(course);
        }


        [HttpPut("update-enrollment")]
        public async Task<IActionResult> UpdateEnrollmentStatus([FromBody] UpdateEnrollmentStatusDto dto)
        {
            var result = await _service.UpdateEnrollmentStatusAsync(dto);

            if (!result)
                return NotFound("StudentCourse not found.");

            return Ok("Enrollment status updated successfully.");
        }
    }
}