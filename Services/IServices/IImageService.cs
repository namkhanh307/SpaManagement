using Repos.Entities;
using Repos.ViewModels;
using Repos.ViewModels.ImageVM;

namespace Services.IServices
{
    public interface IImageService
    {
        Task<PagingVM<GetImageVM>> GetImages(int pageNumber = 1, int pageSize = 10);
        Task PostImage(PostImageVM model);
        Task DeleteImage(string imageId);
    }
}
