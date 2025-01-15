using Core.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Repos.ViewModels.ProductVM;
using Repos.ViewModels;
using Services.IServices;
using System.Runtime.CompilerServices;
using Repos.ViewModels.ImageVM;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController(IImageService imageService) : ControllerBase
    {
        private readonly IImageService _imageService = imageService;

        [HttpGet("get")]
        public async Task<IActionResult> GetImages(int pageNumber = 1, int pageSize = 10)
        {
            PagingVM<GetImageVM> result = await _imageService.GetImages(pageNumber, pageSize);
            return Ok(new BaseResponseModel<PagingVM<GetImageVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }

        [HttpPost("post")]
        public async Task<IActionResult> PostImages(PostImageVM model)
        {
            await _imageService.PostImage(model);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Upload hình ảnh thành công"));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImage(string imageId)
        {
            await _imageService.DeleteImage(imageId);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Xóa hình ảnh thành công"));
        }
    }
}
