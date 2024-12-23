using AutoMapper;
using Core.Infrastructures;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repos.Entities;
using Repos.IRepos;
using Repos.ViewModels;
using Services.IServices;
using System.Linq.Expressions;
using ModelValidator = Core.Infrastructures.ModelValidator;

namespace Services.Services
{
    public class BaseService<TPostModel, TPutModel, TGetModel, T>(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : IBaseService<TPostModel, TPutModel, TGetModel, T> where TPostModel : class where TPutModel : class where TGetModel : BaseVM where T : BaseEntity
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<IEnumerable<TGetModel>> GetAsync(Func<IQueryable<T>, IQueryable<T>>? include = null, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            string currentUserId = Authentication.GetUserIdFromHttpContextAccessor(_httpContextAccessor);
            IQueryable<T> query = _unitOfWork.GetRepo<T>().Entities;
            if (include != null)
            {
                query = include(query);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            var entities = await query.ToListAsync();
            var response = entities.Select(item =>
            {
                var result = _mapper.Map<TGetModel>(item);
                if (item.CreatedBy == currentUserId)
                {
                    dynamic dynamicResult = result as dynamic;
                    if (dynamicResult != null)
                    {
                        dynamicResult.CanDelete = true;
                        dynamicResult.CanUpdate = true;
                    }
                }
                return result;
            });
            return response;
        }

        public async Task<TGetModel> GetByIdAsync(string id, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _unitOfWork.GetRepo<T>().Entities;
            if (include != null)
            {
                query = include(query);
            }
            return _mapper.Map<TGetModel>(await query.FirstOrDefaultAsync(e => EF.Property<string>(e, "Id") == id));
        }

        public async Task PostAsync(TPostModel model)
        {
            string currentUserId = Authentication.GetUserIdFromHttpContextAccessor(_httpContextAccessor);
            try
            {
                ModelValidator.ValidateModel(model);
                var entity = _mapper.Map<T>(model);
                if (entity is BaseEntity baseEntity)
                {
                    baseEntity.CreatedBy = currentUserId;
                    baseEntity.CreatedAt = DateTime.Now;
                }
                await _unitOfWork.GetRepo<T>().Insert(entity);
                await _unitOfWork.Save();
            }
            catch (DbUpdateException ex)
            {
                BaseService<TPostModel, TPutModel, TGetModel, T>.HandleDbUpdateException(ex, typeof(T));
            }
        }


        public async Task PutAsync(string id, TPutModel model)
        {
            string currentUserId = Authentication.GetUserIdFromHttpContextAccessor(_httpContextAccessor);
            try
            {
                ModelValidator.ValidateModel(model);
                var repository = _unitOfWork.GetRepo<T>();
                var entity = await repository.GetById(id);

                if (entity == null)
                    throw new KeyNotFoundException("Entity not found.");

                _mapper.Map(model, entity);
                if (entity is BaseEntity baseEntity)
                {
                    baseEntity.UpdatedBy = currentUserId;
                }
                await repository.Update(entity);
                await _unitOfWork.Save();
            }
            catch (DbUpdateException ex)
            {
                BaseService<TPostModel, TPutModel, TGetModel, T>.HandleDbUpdateException(ex, typeof(T));
            }
        }
        public async Task DeleteAsync(string id)
        {
            try
            {
                if (id == null)
                    throw new ErrorException(StatusCodes.Status400BadRequest,
                ErrorCode.BadRequest, "Vui lòng chọn một đối tượng!");

                await _unitOfWork.GetRepo<T>().Delete(id);
                await _unitOfWork.Save();
            }
            catch (DbUpdateException ex)
            {
                BaseService<TPostModel, TPutModel, TGetModel, T>.HandleDbUpdateException(ex, typeof(T));
            }
        }
        private static void HandleDbUpdateException(DbUpdateException ex, Type entityType)
        {
            if (ex.InnerException?.Message.Contains("PRIMARY KEY") == true)
            {
                throw new ErrorException(
                    StatusCodes.Status409Conflict,
                    ErrorCode.Conflicted,
                    $"Primary key conflict in {entityType.Name}."
                );
            }
            if (ex.InnerException?.Message.Contains("FOREIGN KEY") == true)
            {
                throw new ErrorException(
                    StatusCodes.Status409Conflict,
                    ErrorCode.Conflicted,
                    $"Foreign key constraint violation in {entityType.Name}."
                );
            }

            throw new ErrorException(
                StatusCodes.Status400BadRequest,
                ErrorCode.BadRequest,
                $"Database update error in {entityType.Name}: {ex.Message}"
            );
        }
    }
}
