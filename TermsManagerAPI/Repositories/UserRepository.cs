using Microsoft.EntityFrameworkCore;
using CGUManagementAPI.Data;
using CGUManagementAPI.Models;

namespace CGUManagementAPI.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly CGUManagementDbContext _context;

        public UserRepository(CGUManagementDbContext context) : base(context)
        {
            _context = context;
        }

        // Méthode spécifique pour récupérer un utilisateur par email
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}