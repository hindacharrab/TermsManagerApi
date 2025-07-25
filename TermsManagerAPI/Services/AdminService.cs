using AutoMapper;
using BCrypt.Net;
using CGUManagementAPI.Models;
using TermsManagerAPI.Dtos;
using TermsManagerAPI.Helpers;
using TermsManagerAPI.Repositories.Interface;
using TermsManagerAPI.Services.Interface;

namespace CGUManagementAPI.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICGURepository _cguRepository;
        private readonly IMapper _mapper;

        public AdminService(IUserRepository userRepository, ICGURepository cguRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _cguRepository = cguRepository;
            _mapper = mapper;
        }

        // ✅ Utilise AutoMapper
        public async Task<User> AddUserAsync(CreateUserRequest request)
        {
            var user = UserHelper.CreateUserFromRequest(request, _mapper);

            return await _userRepository.AddAsync(user);
        }
        //UpdateUser

        public async Task<User> ModifierUserAsync(int id, User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
                throw new ArgumentException("Utilisateur non trouvé.");

            _mapper.Map(existingUser, user);

            if (!string.IsNullOrEmpty(user.PasswordHash))
                existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

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

        public async Task<IEnumerable<UserWithCGUStatusDto>> VoirAcceptationsCGUAsync()
        {
            var users = await _userRepository.SearchUsersAsync(includeAcceptedCGU: true);
            var latestCGU = await _cguRepository.GetLatestVersionAsync();
            var currentVersion = latestCGU?.Version ?? "1.0";
            /*******/

            var dtos = UserHelper.MapUsersWithCGUStatus(users, currentVersion, _mapper);
            return dtos;
            /*******/
        }

        public async Task<List<User>> SearchUsersAsync(string? email, string? nom, string? prenom, string? role)
        {
            return await _userRepository.SearchUsersAsync(email, nom, prenom, role);
        }
    }
}
