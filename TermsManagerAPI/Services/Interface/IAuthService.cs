using CGUManagementAPI.Models;

namespace TermsManagerAPI.Services.Interface
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string email, string password);
        Task<string> GenerateJWTAsync(User user);
        Task<bool> ValidateTokenAsync(string token);
        Task LogoutAsync();
    }
}
