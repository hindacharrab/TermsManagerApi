using CGUManagementAPI.Dtos;
using CGUManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TermsManagerAPI.Dtos;
using TermsManagerAPI.Repositories.Interface;
using TermsManagerAPI.Helpers; // ✅ Ajout pour accéder à UserHelper
using System.Security.Claims;
using AutoMapper;

namespace CGUManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICGURepository _cguRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, ICGURepository cguRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _cguRepository = cguRepository;
            _mapper = mapper;
        }

        // GET: api/user/me
        [HttpGet("My Profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var user = await _userRepository.GetByIdAsync(userIdFromToken);
            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable." });

            var latestCGU = await _cguRepository.GetLatestVersionAsync();
            var currentCGUVersion = latestCGU?.Version ?? "1.0";

            var profile = _mapper.Map<UserProfileDto>(user);
            profile.RequiresCGUAcceptance = UserHelper.RequiresCGUAcceptance(user, currentCGUVersion); // ✅ Utilise helper

            return Ok(profile);
        }

        // GET: api/user/cgu-status
        [HttpGet("cgu-status")]
        public async Task<IActionResult> GetCGUStatus()
        {
            var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var user = await _userRepository.GetByIdAsync(userIdFromToken);
            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable." });

            var latestCGU = await _cguRepository.GetLatestVersionAsync();
            var currentCGUVersion = latestCGU?.Version ?? "1.0";

            var status = UserHelper.BuildCGUStatus(user, currentCGUVersion); // ✅ Utilise helper

            return Ok(status);
        }

        // GET: api/user/with-cgu/{id}
        [HttpGet("with-cgu/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserWithCGU(int id)
        {
            var user = await _userRepository.GetUserWithCGUAsync(id);
            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.Id,
                user.Email,
                user.Nom,
                user.Prenom,
                user.Role,
                user.AcceptedCGU?.Version,
                user.AcceptedCGU?.DatePublication,
                user.LastCGUAcceptanceDate
            });
        }

        // PUT: api/user/profile
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdFromToken = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var user = await _userRepository.GetByIdAsync(userIdFromToken);
            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable." });

            _mapper.Map(request, user);

            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            }

            await _userRepository.UpdateAsync(user);

            return Ok(new { message = "Profil mis à jour avec succès." });
        }
    }
}
