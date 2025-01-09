using Repos.DbContextFactory;
using Repos.IRepos;

namespace Repos.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SpaManagementContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new();
        private bool disposed = false;

        public UnitOfWork(SpaManagementContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepo<T> GetRepo<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IGenericRepo<T>)_repositories[typeof(T)];
            }

            var repositoryInstance = new GenericRepo<T>(_dbContext);
            _repositories.Add(typeof(T), repositoryInstance);
            return repositoryInstance;
        }
        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void BeginTransaction()
        {
            _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _dbContext.Database.CommitTransaction();
        }

        public void RollBack()
        {
            _dbContext.Database.RollbackTransaction();
        }

    }
}