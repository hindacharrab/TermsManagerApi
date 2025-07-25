using CGUManagementAPI.Models;

namespace TermsManagerAPI.Repositories.Interface
{
    public interface ICGURepository : IGenericRepository<CGU>
    {
        Task<CGU?> GetLatestVersionAsync();
        Task<CGU?> GetByVersionAsync(string version);
        Task<List<CGU>> GetAllVersionsOrderedAsync();
        Task<bool> VersionExistsAsync(string version);
        Task<List<CGU>> GetPublishedAfterAsync(DateTime date);
    }
}
