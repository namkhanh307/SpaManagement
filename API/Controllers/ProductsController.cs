using Core.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Repos.Entities;
using Repos.ViewModels;
using Repos.ViewModels.ProductVM;
using Services.IServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IBaseService<PostProductVM, PutProductVM, GetProductsVM, Product> baseService) : ControllerBase
    {
        private readonly IBaseService<PostProductVM, PutProductVM, GetProductsVM, Product> _baseService = baseService;
        [HttpGet("get")]
        public async Task<IActionResult> GetProducts(int pageNumber = 1, int pageSize = 10)
        {
            PagingVM<GetProductsVM> result = await _baseService.GetAsync(null, null, null, pageNumber, pageSize);
            return Ok(new BaseResponseModel<PagingVM<GetProductsVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }
        [HttpPost("post")]
        public async Task<IActionResult> PostProduct(PostProductVM model)
        {
            await _baseService.PostAsync(model);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Thêm sản phẩm mới thành công"));
        }
        [HttpPut]
        public async Task<IActionResult> PutProduct(string id, PutProductVM model)
        {
            await _baseService.PutAsync(id, model);
            return Ok(new BaseResponseModel<string>(
               statusCode: StatusCodes.Status200OK,
               code: ResponseCodeConstants.SUCCESS,
               data: "Sửa sản phẩm mới thành công"));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _baseService.DeleteAsync(id);
            return Ok(new BaseResponseModel<string>(
              statusCode: StatusCodes.Status200OK,
              code: ResponseCodeConstants.SUCCESS,
              data: "Xóa sản phẩm thành công"));
        }
    }

}
