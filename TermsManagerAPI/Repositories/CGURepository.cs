using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CGUManagementAPI.Data;
using CGUManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CGUManagementAPI.Repositories
{
    public class CGURepository : Repository<CGU>, ICGURepository
    {
        private readonly CGUManagementDbContext _context;

        public CGURepository(CGUManagementDbContext context) : base(context)
        {
            _context = context;
        }

        // Récupère la dernière version des CGU publiée (la plus récente par date)
        public async Task<CGU?> GetLatestVersionAsync()
        {
            return await _context.CGUs
                .OrderByDescending(c => c.DatePublication)
                .FirstOrDefaultAsync();
        }

        // Récupère une version spécifique des CGU par numéro de version
        public async Task<CGU?> GetByVersionAsync(string version)
        {
            return await _context.CGUs
                .FirstOrDefaultAsync(c => c.Version == version);
        }

        // Récupère toutes les versions des CGU triées par date de publication (plus récente en premier)
        public async Task<List<CGU>> GetAllVersionsOrderedAsync()
        {
            return await _context.CGUs
                .OrderByDescending(c => c.DatePublication)
                .ToListAsync();
        }

        // Vérifie si une version spécifique existe
        public async Task<bool> VersionExistsAsync(string version)
        {
            return await _context.CGUs
                .AnyAsync(c => c.Version == version);
        }

        // Récupère les CGU publiées après une date donnée
        public async Task<List<CGU>> GetPublishedAfterAsync(DateTime date)
        {
            return await _context.CGUs
                .Where(c => c.DatePublication > date)
                .OrderByDescending(c => c.DatePublication)
                .ToListAsync();
        }
    }
}
