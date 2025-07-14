using CGUManagementAPI.Dtos;
using CGUManagementAPI.Models;
using CGUManagementAPI.Repositories;
using CGUManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CGUManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public AuthController(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        // ✅ POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.LoginAsync(request.Email, request.Password);
                return Ok(new { token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        // ✅ POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "Cet email est déjà utilisé." });
            }

            var user = new User
            {
                Email = request.Email,
                Nom = request.Nom,
                Prenom = request.Prenom,
                Role = "User",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                DateCreation = DateTime.UtcNow
            };

            var createdUser = await _userRepository.AddAsync(user);

            return Ok(new
            {
                message = "Inscription réussie.",
                user = new
                {
                    createdUser.Id,
                    createdUser.Email,
                    createdUser.Nom,
                    createdUser.Prenom,
                    createdUser.Role
                }
            });
        }

        // ✅ POST: api/auth/validate
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateToken([FromBody] string token)
        {
            var isValid = await _authService.ValidateTokenAsync(token);
            return Ok(new { valid = isValid });
        }

        // ✅ POST: api/auth/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok(new { message = "Déconnexion effectuée" });
        }
    }
}
