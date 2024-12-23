namespace Repos.IRepos
{
    public interface IGenericRepo<T> where T : class
    {
        IQueryable<T> Entities { get; }
        Task<T?> GetById(object obj);
        Task InsertCollection(ICollection<T> entities);
        Task Insert(T entity);
        Task Update(T entity);
        Task UpdateCollection(ICollection<T> entity);
        Task Delete(object id);
        Task DeleteCollection(ICollection<T> entities);
    }
}
