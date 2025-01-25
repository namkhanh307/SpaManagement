using AutoMapper;
using Core.Infrastructures;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repos.Entities;
using Repos.IRepos;
using Repos.ViewModels;
using Repos.ViewModels.ImageVM;
using Services.IServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Services.Services
{
    public class ImageService(IUnitOfWork unitOfWork, IMapper mapper) : IImageService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<PagingVM<GetImageVM>> GetImages(int pageNumber = 1, int pageSize = 10)
        {
            IQueryable<Image> query = _unitOfWork.GetRepo<Image>().Entities;
            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var entities = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var response = new List<GetImageVM>();
            foreach (var item in entities)
            {
                var result = _mapper.Map<GetImageVM>(item);
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

            return new PagingVM<GetImageVM>
            {
                List = response,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
            };
        }
        public async Task PostImage(PostImageVM model)
        {
            if (model.Image is null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Vui lòng upload ảnh!");
            }
            foreach (IFormFile file in model.Image)
            {
                string id = Guid.NewGuid().ToString("N");
                Image image = new()
                {
                    Id = id,
                    Url = await FileUploadHelper.UploadFile(file, id)
                };
                await _unitOfWork.GetRepo<Image>().Insert(image);
            }
            await _unitOfWork.Save();
        }

        public async Task DeleteImage(string imageId)
        {
            string url = _unitOfWork.GetRepo<Image>().GetById(imageId).Result.Url;
            FileUploadHelper.DeleteFile(url);
            await _unitOfWork.GetRepo<Image>().Delete(imageId);
            await _unitOfWork.Save();
        }
    }
}
