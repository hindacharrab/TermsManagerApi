using CGUManagementAPI.Models;

namespace TermsManagerAPI.Repositories.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetAllAsync(bool includeAcceptedCGU = false);
        Task<List<User>> SearchUsersAsync(
            string? email = null,
            string? nom = null,
            string? prenom = null,
            string? role = null,
            bool includeAcceptedCGU = false
        );
        Task<User?> GetUserWithCGUAsync(int id);
    }
}
