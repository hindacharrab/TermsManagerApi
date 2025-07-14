using CGUManagementAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CGUManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CGUController : ControllerBase
    {
        private readonly ICGURepository _cguRepository;
        private readonly IUserRepository _userRepository;

        public CGUController(ICGURepository cguRepository, IUserRepository userRepository)
        {
            _cguRepository = cguRepository;
            _userRepository = userRepository;
        }

        // ✅ GET: api/cgu/latest
        [HttpGet("latest")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLatest()
        {
            var cgu = await _cguRepository.GetLatestVersionAsync();
            if (cgu == null)
                return NotFound(new { message = "Aucune CGU disponible." });

            return Ok(cgu);
        }

        // ✅ GET: api/cgu/all
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllVersions()
        {
            var all = await _cguRepository.GetAllVersionsOrderedAsync();
            return Ok(all);
        }

        // ✅ POST: api/cgu/accept
        [HttpPost("accept")]
        [Authorize]
        public async Task<IActionResult> AcceptLatestCGU()
        {
            // 🔐 1. Récupérer l'ID utilisateur depuis le token JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new { message = "Utilisateur non authentifié." });

            int userId = int.Parse(userIdClaim.Value);

            // 📄 2. Récupérer la dernière version de la CGU
            var latestCGU = await _cguRepository.GetLatestVersionAsync();
            if (latestCGU == null)
                return NotFound(new { message = "Aucune CGU disponible." });

            // 👤 3. Récupérer l'utilisateur
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable." });

            // 🖊️ 4. Mettre à jour la date d’acceptation
            user.LastCGUAcceptanceDate = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            return Ok(new { message = "CGU acceptée avec succès." });
        }
    }
}
