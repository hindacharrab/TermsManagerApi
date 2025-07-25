using CGUManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TermsManagerAPI.Repositories.Interface;

namespace CGUManagementAPI.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly CGUManagementDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(CGUManagementDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id) =>
            await _dbSet.FindAsync(id);

        public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            foreach (var include in includes)
                query = query.Include(include);
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
                query = query.Where(filter);
            return await query.ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
                                               Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                               params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var include in includes)
                query = query.Include(include);

            return orderBy != null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
        }

        // Implémentation de l'interface (version simple avec 2 paramètres)
        public async Task<List<TResult>> GetAllAsync<TResult>(
            Expression<Func<T, bool>>? filter,
            Expression<Func<T, TResult>> selector)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            return await query.Select(selector).ToListAsync();
        }

        // Surcharge avec paramètres supplémentaires (orderBy et includes)
        public async Task<List<TResult>> GetAllAsync<TResult>(
            Expression<Func<T, bool>>? filter,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var include in includes)
                query = query.Include(include);

            if (orderBy != null)
                query = orderBy(query);

            return await query.Select(selector).ToListAsync();
        }

        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter) =>
            await _dbSet.FirstOrDefaultAsync(filter);

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter) =>
            await _dbSet.AnyAsync(filter);

        public async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null) =>
            filter != null ? await _dbSet.CountAsync(filter) : await _dbSet.CountAsync();

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> AddRangeAsync(List<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(); 
        }

        public async Task DeleteRangeAsync(List<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}