using AutoMapper;
using CGUManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TermsManagerAPI.Dtos;
using TermsManagerAPI.Helpers;
using TermsManagerAPI.Services.Interface;

namespace CGUManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // 🔐 Seuls les admins ont accès à ce contrôleur
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;


        public AdminController(IAdminService adminService, IMapper mapper)
        {
            _adminService = adminService;
            _mapper = mapper;
        }

        // ✅ Ajouter un utilisateur
        [HttpPost("users")]
        public async Task<IActionResult> AddUser([FromBody] CreateUserRequest request)
        {
            // virefier est ce que le champs Role est present et non vide!!!

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _adminService.AddUserAsync(request);

            var userDto = _mapper.Map<UserWithCGUStatusDto>(user);

            // Complète les propriétés calculées liées aux CGU et admin
            userDto.RequiresCGUAcceptance = UserHelper.RequiresCGUAcceptance(user, currentCGUVersion: "1.0"); // adapte ta version CGU ici
            userDto.AcceptedByAdmin = UserHelper.IsAdmin(user);

            return Ok(userDto);
        }



        // ✅ Modifier un utilisateur
        [HttpPut("users/{id}")]
        public async Task<IActionResult> ModifierUser(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updated = await _adminService.ModifierUserAsync(id, user);
                return Ok(updated);
            }
            catch (ArgumentException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        // ✅ Supprimer un utilisateur
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _adminService.DeleteUserAsync(id);
            return NoContent();
        }

        // ✅ Ajouter une nouvelle CGU
        [HttpPost("cgu")]
        public async Task<IActionResult> AddNewCGU([FromBody] CGU cgu)
        {
            var newCGU = await _adminService.AddNewCGUAsync(cgu);
            return Ok(newCGU);
        }

        // ✅ Voir la liste des utilisateurs avec acceptation CGU
        [HttpGet("users/cgu-status")]
        public async Task<IActionResult> VoirAcceptationsCGU()
        {
            var users = await _adminService.VoirAcceptationsCGUAsync();
            return Ok(users);
        }




        // 🔎 Rechercher des utilisateurs
        [HttpGet("users/search")]
        public async Task<IActionResult> SearchUsers(
            [FromQuery] string? email,
            [FromQuery] string? nom,
            [FromQuery] string? prenom,
            [FromQuery] string? role)
        {
            var results = await _adminService.SearchUsersAsync(email, nom, prenom, role);
            return Ok(results);
        }
    }
}
