using CGUManagementAPI.Models;
using TermsManagerAPI.Dtos;

namespace TermsManagerAPI.Services.Interface
{
    public interface IAdminService
    {
        Task<User> AddUserAsync(CreateUserRequest request);
        Task<User> ModifierUserAsync(int id, User user);
        Task DeleteUserAsync(int id);
        Task<CGU> AddNewCGUAsync(CGU cgu);

        // ✅ Type de retour changé pour supporter l’anonymisation
        Task<IEnumerable<UserWithCGUStatusDto>> VoirAcceptationsCGUAsync();
        Task<List<User>> SearchUsersAsync(string? email, string? nom, string? prenom, string? role);


    }
}
