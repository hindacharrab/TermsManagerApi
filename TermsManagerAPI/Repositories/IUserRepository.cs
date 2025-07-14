using CGUManagementAPI.Models;

namespace CGUManagementAPI.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        // Pas besoin de redéfinir GetAllAsync() car elle est héritée de IRepository<User>
    }
}