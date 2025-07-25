using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TermsManagerAPI.Helpers;
using TermsManagerAPI.Repositories.Interface;

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

        // GET: api/cgu/latest
        [HttpGet("latest")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLatest()
        {
            var cgu = await _cguRepository.GetLatestVersionAsync();
            if (cgu == null)
                return NotFound(new { message = "Aucune CGU disponible." });

            return Ok(cgu);
        }

        // GET: api/cgu/all (Admin only)
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllVersions()
        {
            var all = await _cguRepository.GetAllVersionsOrderedAsync();
            return Ok(all);
        }

        
        // POST: api/cgu/accept
        [HttpPost("accept")]
        [Authorize]
        public async Task<IActionResult> AcceptLatestCGU()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new { message = "Utilisateur non authentifié." });

            int userId = int.Parse(userIdClaim.Value);

            var latestCGU = await _cguRepository.GetLatestVersionAsync();
            if (latestCGU == null)
                return NotFound(new { message = "Aucune CGU disponible." });

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable." });

            // Utiliser le helper pour enregistrer la version et la date
            UserHelper.AcceptCGU(user, latestCGU.Version);

            // Lier à la CGU en base
            user.AcceptedCGUId = latestCGU.Id;

            await _userRepository.UpdateAsync(user);

            return Ok(new
            {
                message = "CGU acceptée avec succès.",
                acceptedVersion = latestCGU.Version,
                acceptedAt = user.LastCGUAcceptanceDate
            });
        }



        // POST: api/cgu/check-required
        [HttpPost("check-required")]
        [Authorize]
        public async Task<IActionResult> CheckCGURequired()
        {
          // Vérifie que l'identifiant existe dans les claims. Sinon, retourne une erreur 401
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized(new { message = "Utilisateur non authentifié." });
          // Convertit le claim (string) en entier. Risque d'exception si le format n'est pas correct.
            int userId = int.Parse(userIdClaim.Value);

            // Récupère les informations de l'utilisateur en base de données via son ID
            var user = await _userRepository.GetByIdAsync(userId);
            // Vérifie que l'utilisateur existe. Sinon, retourne une erreur 404.
            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable." });

            var latestCGU = await _cguRepository.GetLatestVersionAsync();
            if (latestCGU == null)
                return NotFound(new { message = "Aucune CGU disponible." });

            // Utilisation du helper pour retourner le statut complet
            var status = UserHelper.BuildCGUStatus(user, latestCGU.Version);

            return Ok(status);
        }

    }
}
