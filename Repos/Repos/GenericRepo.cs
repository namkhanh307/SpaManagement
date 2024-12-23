using Microsoft.EntityFrameworkCore;
using Repos.DbContextFactory;
using Repos.IRepos;

namespace Repos.Repos
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        protected readonly SpaManagementContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepo(SpaManagementContext dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> Entities => _context.Set<T>();

        public async Task<T?> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Insert(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task InsertCollection(ICollection<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }
        public async Task Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCollection(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                _dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }
        public async Task Delete(object id)
        {
            T? entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
        public async Task DeleteCollection(ICollection<T> entities)
        {
            foreach (T entity in entities)
            {
                _dbSet.Remove(entity);
            }
            await _context.SaveChangesAsync();
        }
    }
}
