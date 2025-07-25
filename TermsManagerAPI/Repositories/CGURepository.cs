using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CGUManagementAPI.Data;
using CGUManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using TermsManagerAPI.Repositories.Interface;

namespace CGUManagementAPI.Repositories
{
    // CGURepository hérite du GenericRepository pour CGU
    public class CGURepository : GenericRepository<CGU>, ICGURepository
    {
        // Constructeur qui transmet le contexte EF au GenericRepository
        public CGURepository(CGUManagementDbContext context) : base(context)
        {
        }
        public async Task<CGU?> GetLatestVersionAsync()
        {
            // Utilisation directe du DbSet CGUs pour trier par date et prendre la première (la plus récente)
            return await _context.CGUs
                .OrderByDescending(c => c.DatePublication)
                .FirstOrDefaultAsync();
        }

        // Récupère une CGU en fonction de sa version
        public async Task<CGU?> GetByVersionAsync(string version)
        {
            return await _context.CGUs
                .FirstOrDefaultAsync(c => c.Version == version);
        }

        // Récupère toutes les versions de CGU triées par date décroissante (de la plus récente à la plus ancienne)
        public async Task<List<CGU>> GetAllVersionsOrderedAsync()
        {
            return await _context.CGUs
                .OrderByDescending(c => c.DatePublication)
                .ToListAsync();
        }

        // Vérifie si une version CGU existe dans la base
        public async Task<bool> VersionExistsAsync(string version)
        {
            return await _context.CGUs
                .AnyAsync(c => c.Version == version);
        }

        // Récupère toutes les CGUs publiées après une date donnée (utile pour vérifier les mises à jour)
        public async Task<List<CGU>> GetPublishedAfterAsync(DateTime date)
        {
            return await _context.CGUs
                .Where(c => c.DatePublication > date)
                .OrderByDescending(c => c.DatePublication)
                .ToListAsync();
        }
    }
}
