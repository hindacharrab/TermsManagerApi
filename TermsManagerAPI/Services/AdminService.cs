using CGUManagementAPI.Models;
using CGUManagementAPI.Repositories;
using BCrypt.Net;

namespace CGUManagementAPI.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICGURepository _cguRepository;

        public AdminService(IUserRepository userRepository, ICGURepository cguRepository)
        {
            _userRepository = userRepository;
            _cguRepository = cguRepository;
        }

        public async Task<User> AddUserAsync(User user)
        {
            // Hasher le mot de passe
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            return await _userRepository.AddAsync(user);
        }

        public async Task<User> ModifierUserAsync(int id, User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
                throw new ArgumentException("Utilisateur non trouvé");

            existingUser.Email = user.Email;
            existingUser.Nom = user.Nom;
            existingUser.Prenom = user.Prenom;
            existingUser.Role = user.Role;

            // Si mot de passe modifié
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            }

            return await _userRepository.UpdateAsync(existingUser);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<CGU> AddNewCGUAsync(CGU cgu)
        {
            cgu.DatePublication = DateTime.UtcNow;
            return await _cguRepository.AddAsync(cgu);
        }

        public async Task<IEnumerable<User>> VoirAcceptationsCGUAsync()
        {
            return await _userRepository.GetAllAsync();
        }
    }
}