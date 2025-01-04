using Core.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Repos.Entities;
using Repos.ViewModels;
using Repos.ViewModels.OrderVM;
using Repos.ViewModels.PackageVM;
using Repos.ViewModels.ProductVM;
using Services.IServices;
using Services.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageService(IBaseService<PostPackageVM, PutPackageVM, GetPackagesVM, Package> baseService) : ControllerBase
    {
        private readonly IBaseService<PostPackageVM, PutPackageVM, GetPackagesVM, Package> _baseService = baseService;
        [HttpGet("get")]
        public async Task<IActionResult> GetPackages(int pageNumber = 1, int pageSize = 10)
        {
            PagingVM<GetPackagesVM> result = await _baseService.GetAsync(null, null, null , pageNumber, pageSize);
            return Ok(new BaseResponseModel<PagingVM<GetPackagesVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }
        [HttpPost]
        public async Task<IActionResult> PostPackage(PostPackageVM model)
        {
            await _baseService.PostAsync(model);
            return Ok(new BaseResponseModel<string>(
               statusCode: StatusCodes.Status200OK,
               code: ResponseCodeConstants.SUCCESS,
               data: "Thêm gói sản phẩm mới thành công"));
        }
        [HttpPut]
        public async Task<IActionResult> PutPackage(string id,PutPackageVM model)
        {
            await _baseService.PutAsync(id, model);
            await _baseService.PutAsync(id, model);
            return Ok(new BaseResponseModel<string>(
               statusCode: StatusCodes.Status200OK,
               code: ResponseCodeConstants.SUCCESS,
               data: "Sửa gói sản phẩm thành công"));
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePackage(string id)
        {
            await _baseService.DeleteAsync(id);
            return Ok(new BaseResponseModel<string>(
              statusCode: StatusCodes.Status200OK,
              code: ResponseCodeConstants.SUCCESS,
              data: "Xóa gói sản phẩm thành công"));
        }
    }
}
