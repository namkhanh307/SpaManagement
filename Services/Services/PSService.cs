using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repos.Entities;
using Repos.IRepos;
using Repos.ViewModels;
using Repos.ViewModels.PackageVM;
using Core.Infrastructures;
using Microsoft.AspNetCore.Http;
using Services.IServices;
using System.Linq.Expressions;

namespace Services.Services
{
    public class PSService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : IPSService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<PagingVM<GetPackagesVM>> GetPackages(int pageNumber = 1, int pageSize = 10)
        {
            IQueryable<Package> query = _unitOfWork.GetRepo<Package>().Entities.Include(p => p.PackageServices).ThenInclude(p => p.Service);

            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var entities = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var response = new List<GetPackagesVM>();
            foreach (var item in entities)
            {
                var result = _mapper.Map<GetPackagesVM>(item);
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

            return new PagingVM<GetPackagesVM>
            {
                List = response,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
            };
        }

        public async Task PostPackage(PostPackageVM model)
        {
            string currentUserId = Authentication.GetUserIdFromHttpContextAccessor(_httpContextAccessor);

            // Validate the model
            ModelValidator.ValidateModel(model);

            // Map the model to the entity
            Package entity = _mapper.Map<Package>(model);    

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
            await _unitOfWork.GetRepo<Package>().Insert(entity);

            foreach (PostPackageServiceVM obj in model.Services)
            {
                PackageService packageService = new()
                {
                    PackageId = entity.Id,
                    ServiceId = obj.ServiceId,
                    Quantity = obj.Quantity,
                    Type = obj.Type,
                };
                await _unitOfWork.GetRepo<PackageService>().Insert(packageService);
            }
            await _unitOfWork.Save();
        }

        public async Task PutPackage(string id, PutPackageVM model)
        {
            string currentUserId = Authentication.GetUserIdFromHttpContextAccessor(_httpContextAccessor);
            ModelValidator.ValidateModel(model);
            var repository = _unitOfWork.GetRepo<Package>();
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

        public async Task DeletePackage(string packageId)
        {
            Package? package = await _unitOfWork.GetRepo<Package>().GetById(packageId) ??
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Không tìm thấy package!");

            ICollection<PackageService> packageServices = await _unitOfWork.GetRepo<PackageService>().Entities.Where(p => p.PackageId == packageId).ToListAsync();
            await _unitOfWork.GetRepo<PackageService>().DeleteCollection(packageServices);

            await _unitOfWork.GetRepo<PackageService>().Delete(package);
            await _unitOfWork.Save();
        }
    }
}
