namespace Repos.IRepos
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepo<T> GetRepo<T>() where T : class;
        Task Save();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
    }
}
                                                   