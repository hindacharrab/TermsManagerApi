// Services/IAdminService.cs
using CGUManagementAPI.Models;

namespace CGUManagementAPI.Services
{
    public interface IAdminService
    {
        Task<User> AddUserAsync(User user);
        Task<User> ModifierUserAsync(int id, User user);
        Task DeleteUserAsync(int id);
        Task<CGU> AddNewCGUAsync(CGU cgu);
        Task<IEnumerable<User>> VoirAcceptationsCGUAsync();
    }
}
