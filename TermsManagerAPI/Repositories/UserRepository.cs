using CGUManagementAPI.Data;
using CGUManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TermsManagerAPI.Repositories.Interface;

namespace CGUManagementAPI.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(CGUManagementDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserWithCGUAsync(int id)
        {
            return await _context.Users
                .Include(u => u.AcceptedCGU)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        // Méthode avec même nom que dans GenericRepository mais signature différente
        public async Task<List<User>> GetAllAsync(bool includeAcceptedCGU = false)
        {
            var includes = includeAcceptedCGU
                ? new Expression<Func<User, object>>[] { u => u.AcceptedCGU }
                : Array.Empty<Expression<Func<User, object>>>();

            return await base.GetAllAsync(null, null, includes);
        }


        public async Task<List<User>> SearchUsersAsync(
            string? email = null,
            string? nom = null,
            string? prenom = null,
            string? role = null,
            bool includeAcceptedCGU = false
        )
        {
            var query = _context.Users.AsQueryable();

            if (includeAcceptedCGU)
                query = query.Include(u => u.AcceptedCGU);

            if (!string.IsNullOrEmpty(email))
                query = query.Where(u => u.Email.Contains(email));

            if (!string.IsNullOrEmpty(nom))
                query = query.Where(u => u.Nom.Contains(nom));

            if (!string.IsNullOrEmpty(prenom))
                query = query.Where(u => u.Prenom.Contains(prenom));

            if (!string.IsNullOrEmpty(role))
                query = query.Where(u => u.Role == role);

            return await query.ToListAsync();
        }
    }
}