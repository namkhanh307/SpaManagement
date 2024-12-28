using Repos.Entities;
using Repos.ViewModels;
using System.Linq.Expressions;

namespace Services.IServices
{
    public interface IBaseService<TPostModel, TPutModel, TGetModel, T> where TPostModel : class where TPutModel : class where TGetModel : BaseVM where T : class
    {
        Task<PagingVM<TGetModel>> GetAsync(Func<IQueryable<T>, IQueryable<T>>? include = null, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int pageNumber = 1, int pageSize = 10);
        Task<TGetModel> GetByIdAsync(string id, Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task PostAsync(TPostModel model);
        Task PutAsync(string id, TPutModel model);
        Task DeleteAsync(string id);
    }
}
