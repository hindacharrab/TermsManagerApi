using CGUManagementAPI.Models;

namespace CGUManagementAPI.Repositories
{
    public interface ICGURepository : IRepository<CGU>
    {
        /// <summary>
        /// Récupère la dernière version des CGU publiée
        /// </summary>
        Task<CGU?> GetLatestVersionAsync();

        /// <summary>
        /// Récupère une version spécifique des CGU par numéro de version
        /// </summary>
        Task<CGU?> GetByVersionAsync(string version);

        /// <summary>
        /// Récupère toutes les versions des CGU triées par date de publication (plus récente en premier)
        /// </summary>
        Task<List<CGU>> GetAllVersionsOrderedAsync();

        /// <summary>
        /// Vérifie si une version spécifique existe
        /// </summary>
        Task<bool> VersionExistsAsync(string version);

        /// <summary>
        /// Récupère les CGU publiées après une date donnée
        /// </summary>
        Task<List<CGU>> GetPublishedAfterAsync(DateTime date);
    }
}