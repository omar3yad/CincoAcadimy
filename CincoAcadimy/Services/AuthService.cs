using CincoAcadimy.DTOs;
using CincoAcadimy.Models;
using CincoAcadimy.Repository.@interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CincoAcadimy.IServices;


namespace CincoAcadimy.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly ApplicationDbContext _context;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration config, IUserRoleRepository userRoleRepository, ApplicationDbContext context)
        {
            _userManager = userManager;
            _config = config;
            _userRoleRepository = userRoleRepository;
            _context = context;
        }

        public async Task<string> RegisterAsync(RegisterDto model)
        {
            // ensure unique email
            var existingByEmail = await _userManager.FindByEmailAsync(model.Email);
            if (existingByEmail != null)
            {
                return "This email is already registered. Please use another email.";
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                FullName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.Phone
            };
            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!await _userManager.IsInRoleAsync(user, "Student"))
                    {
                        await _userManager.AddToRoleAsync(user, "Student");
                    }
                    return "User Registered Successfully with Student Role";

                }
                return string.Join(" | ", result.Errors.Select(e => e.Description));
            }
            catch (Exception ex)
            {
                return ex.InnerException?.Message ?? ex.Message;
            }

        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                throw new UnauthorizedAccessException("Email and password are required");

            var user = await _userManager.FindByEmailAsync(model.Email);
            // Load student
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.UserId == user.Id);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                throw new UnauthorizedAccessException("Invalid username or password");

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName)
        };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var secretKey = _config["JWT:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JWT:IssuerIP"],
                audience: _config["JWT:AudienceIP"],
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: creds);


            return new DTOs.AuthResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                Role = roles.FirstOrDefault(),
                StudentId = student.Id,  // ✅ كده تمام
                StudentName = user.UserName
            };


        }

        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.UserName,
                Email = u.Email,
                //CompanyName = u.Company != null ? u.Company.Name : null
            });
        }


        public async Task<string> ChangeUserRoleAsync(string userId, string newRole)
        {
            var user = await _userRoleRepository.GetUserByIdAsync(userId);
            if (user == null) return "User not found";

            var currentRoles = await _userRoleRepository.GetUserRolesAsync(user);

            // Remove old roles
            foreach (var role in currentRoles)
            {
                await _userRoleRepository.RemoveFromRoleAsync(user, role);
            }

            // Add new role
            var result = await _userRoleRepository.AddToRoleAsync(user, newRole);
            if (result.Succeeded)
                return $"User {user.UserName} role changed to {newRole}";

            return string.Join(" | ", result.Errors.Select(e => e.Description));
        }
        public async Task<List<InstructorDto>> GetAllInstructorsAsync()
        {
            var instructors = await _userRoleRepository.GetAllAsync();

            return instructors.Select(i => new InstructorDto
            {
                Id = i.Id,
                Name = i.User?.FullName ?? "Unknown",
                Email = i.User?.Email ?? "N/A",
                Phone = i.User.PhoneNumber,
                Specialization = i.Specialization,
                CoursesCount = i.Courses?.Count ?? 0
            }).ToList();
        }
        public async Task<StudentDashboardDto> GetDashboardAsync(int studentId)
        {
            return await _userRoleRepository.GetDashboardDataAsync(studentId);
        }
    }
}