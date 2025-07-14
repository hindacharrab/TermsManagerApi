using CGUManagementAPI.Models;

namespace CGUManagementAPI.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string email, string password);
        Task<bool> ValidateTokenAsync(string token);
        Task<string> GenerateJWTAsync(User user);
        Task LogoutAsync();
    }
}