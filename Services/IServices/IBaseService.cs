using System.Linq.Expressions;

namespace Services.IServices
{
    public interface IBaseService<TPostModel, TPutModel, TGetModel, T>
    {
        Task<IEnumerable<TGetModel>> GetAsync(Func<IQueryable<T>, IQueryable<T>>? include = null, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
        Task<TGetModel> GetByIdAsync(string id, Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task PostAsync(TPostModel model);
        Task PutAsync(string id, TPutModel model);
        Task DeleteAsync(string id);
    }
}
