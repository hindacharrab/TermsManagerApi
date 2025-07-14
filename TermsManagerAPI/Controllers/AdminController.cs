using CGUManagementAPI.Models;
using CGUManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CGUManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // ✅ Ajouter un utilisateur
        [HttpPost("users")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            var newUser = await _adminService.AddUserAsync(user);
            return Ok(newUser);
        }

        // ✅ Modifier un utilisateur
        [HttpPut("users/{id}")]
        public async Task<IActionResult> ModifierUser(int id, [FromBody] User user)
        {
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
        [HttpGet("users")]
        public async Task<IActionResult> VoirAcceptationsCGU()
        {
            var users = await _adminService.VoirAcceptationsCGUAsync();
            return Ok(users);
        }
    }
}
