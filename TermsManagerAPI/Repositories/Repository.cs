using CGUManagementAPI.Data;

namespace CGUManagementAPI.Repositories
{
    // Classe intermédiaire pour centraliser d'éventuelles fonctionnalités communes
    public class Repository<T> : GenericRepository<T> where T : class
    {
        public Repository(CGUManagementDbContext context) : base(context)
        {
        }
    }
}
 