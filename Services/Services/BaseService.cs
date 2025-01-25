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
    public class BaseService<TPostModel, TPutModel, TGetModel, T>(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : IBaseService<TPostModel, TPutModel, TGetModel, T> where TPostModel : class where TPutModel : class where TGetModel : BaseVM where T : class
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<PagingVM<TGetModel>> GetAsync(Func<IQueryable<T>, IQueryable<T>>? include = null, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int pageNumber = 1, int pageSize = 10)
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

            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var entities = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var response = new List<TGetModel>();
            foreach (var item in entities)
            {
                var result = _mapper.Map<TGetModel>(item);
                if (item is BaseEntity baseEntity)
                {
                    var user = await _unitOfWork.GetRepo<User>().GetById(baseEntity.CreatedBy ?? "");
                    result.CreatedBy = user?.FullName ?? "";
                }
                if (item is IHasAttribute hasAttribute)
                {
                    var user = await _unitOfWork.GetRepo<User>().GetById(hasAttribute.CreatedBy ?? "");
                    result.GetType().GetProperty("CreatedBy")?.SetValue(result, user?.FullName ?? "");
                }
                response.Add(result);
            }

            return new PagingVM<TGetModel>
            {
                List = response,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
            };
        }


        public async Task<TGetModel> GetByIdAsync(string id, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _unitOfWork.GetRepo<T>().Entities;
            if (include != null)
            {
                query = include(query);
            }
            T? result = await query.FirstOrDefaultAsync(e => EF.Property<string>(e, "Id") == id);
            if (result != null)
            {
                if (result is BaseEntity baseEntity)
                {
                    Task<User?> user = _unitOfWork.GetRepo<User>().GetById(baseEntity.CreatedBy != null ? baseEntity.CreatedBy : "");
                    baseEntity.CreatedBy = user.Result != null ? user.Result.FullName : "";
                }
                if (result is IHasAttribute hasAttribute)
                {
                    var user = await _unitOfWork.GetRepo<User>().GetById(hasAttribute.CreatedBy ?? "");
                    result.GetType().GetProperty("CreatedBy")?.SetValue(result, user?.FullName ?? "");
                }
                return _mapper.Map<TGetModel>(result);
            }
            else
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.BadRequest, "Không tồn tại thực thể!");
            }
        }


        public async Task PostAsync(TPostModel model, List<Dictionary<string, string>>? foreignKeyChecks)
        {
            string currentUserId = Authentication.GetUserIdFromHttpContextAccessor(_httpContextAccessor);

            try
            {
                // Validate the model
                ModelValidator.ValidateModel(model);

                // Map the model to the entity
                T entity = _mapper.Map<T>(model);

                // Perform foreign key checks
                if (foreignKeyChecks != null)
                {
                    foreach (var check in foreignKeyChecks)
                    {
                        foreach (var (foreignKey, errorMessage) in check)
                        {
                            var foreignKeyProperty = entity.GetType().GetProperty(foreignKey);
                            if (foreignKeyProperty != null)
                            {
                                var foreignKeyValue = foreignKeyProperty.GetValue(entity)?.ToString();

                                if (!string.IsNullOrWhiteSpace(foreignKeyValue))
                                {
                                    // Retrieve the property name as a string
                                    string foreignKeyPropertyName = foreignKeyProperty.Name;

                                    // Check if the foreign key exists
                                    bool foreignKeyExists = await _unitOfWork.GetRepo<T>().Entities
                                        .AnyAsync(e => EF.Property<string>(e, foreignKeyPropertyName) == foreignKeyValue);

                                    if (foreignKeyExists)
                                    {
                                        throw new ErrorException(
                                            StatusCodes.Status409Conflict,
                                            ErrorCode.Conflicted,
                                            errorMessage.Replace("{Key}", foreignKey).Replace("{Value}", foreignKeyValue)
                                        );
                                    }
                                }
                            }
                        }
                    }
                }

                // Add created by and created at properties for auditable entities
                if (entity is BaseEntity baseEntity)
                {
                    baseEntity.CreatedBy = currentUserId;
                    baseEntity.CreatedAt = DateTime.Now;
                }

                if (entity is IHasAttribute hasAttribute)
                {
                    hasAttribute.GetType().GetProperty("CreatedBy")?.SetValue(hasAttribute, currentUserId);
                    hasAttribute.GetType().GetProperty("CreatedAt")?.SetValue(hasAttribute, DateTime.Now);
                }

                // Insert the entity and save changes
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
                var entity = await repository.GetById(id) ?? throw new KeyNotFoundException("Entity not found.");
                _mapper.Map(model, entity);
                if (entity is BaseEntity baseEntity)
                {
                    baseEntity.UpdatedBy = currentUserId;
                }
                if (entity is IHasAttribute hasAttribute)
                {
                    hasAttribute.GetType().GetProperty("UpdatedBy")?.SetValue(hasAttribute, currentUserId);
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
