using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using CincoAcadimy.IServices;

using CincoAcadimy.DTOs;

namespace CincoAcadimy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
            private readonly IAuthService _authService;

            public AccountController(IAuthService authService)
            {
                _authService = authService;
            }


            [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] RegisterDto model)
            {
                try
                {
                    var result = await _authService.RegisterAsync(model);
                    return Ok(new { message = result });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { error = ex.Message });
                }
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login([FromBody] LoginDto model)
            {
                try
                {
                    var result = await _authService.LoginAsync(model);
                    return Ok(result);
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Unauthorized(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { error = ex.Message });
                }
            }

            [HttpGet("by-role/{role}")]
            public async Task<IActionResult> GetUsersByRole(string role)
            {
                var users = await _authService.GetUsersByRoleAsync(role);
                return Ok(users);
            }

        [HttpPost("ChangeRole")]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            var result = await _authService.ChangeUserRoleAsync(userId, newRole);

            if (result.Contains("not found") || result.Contains("error", StringComparison.OrdinalIgnoreCase))
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("instructor")]
        public async Task<IActionResult> GetAll()
        {
            var instructors = await _authService.GetAllInstructorsAsync();

            if (instructors == null || instructors.Count == 0)
                return NotFound(new { message = "No instructors found" });

            return Ok(instructors);
        }

        [HttpGet("dashboard/{studentId}")]
        public async Task<IActionResult> GetDashboard(int studentId)
        {
            var result = await _authService.GetDashboardAsync(studentId);
            return Ok(result);
        }



    }
}
