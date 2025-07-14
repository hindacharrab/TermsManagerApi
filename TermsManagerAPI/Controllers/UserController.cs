using CGUManagementAPI.Models;
using CGUManagementAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using TermsManagerAPI.Dtos;  // Assure-toi que ce namespace est correct
using System;
using System.Threading.Tasks;

namespace CGUManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICGURepository _cguRepository;

        public UserController(IUserRepository userRepository, ICGURepository cguRepository)
        {
            _userRepository = userRepository;
            _cguRepository = cguRepository;
        }

        // POST: api/user/{id}/accept-cgu
        [HttpPost("{id}/accept-cgu")]
        public async Task<IActionResult> AcceptCGU(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable." });

            var latestCGU = await _cguRepository.GetLatestVersionAsync();
            if (latestCGU == null)
                return NotFound(new { message = "Aucune CGU disponible." });

            user.LastCGUAcceptanceDate = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            return Ok(new
            {
                message = "CGU acceptée.",
                acceptedAt = user.LastCGUAcceptanceDate,
                cguVersion = latestCGU.Version
            });
        }

        // PUT: api/user/{id}/profile
        [HttpPut("{id}/profile")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "Utilisateur introuvable." });

            // Mise à jour des champs Nom et Prenom
            user.Nom = request.Nom;
            user.Prenom = request.Prenom;

            // Mise à jour du mot de passe si fourni
            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                user.PasswordHash = HashPassword(request.NewPassword);
            }

            await _userRepository.UpdateAsync(user);

            return Ok(new { message = "Profil mis à jour avec succès." });
        }

        // Méthode basique de hashage (à remplacer par une vraie méthode sécurisée)
        private string HashPassword(string password)
        {
            // Exemple simple, à remplacer par un vrai hashage (ex: BCrypt)
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
