using Core.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Repos.Entities;
using Repos.ViewModels.ProductVM;
using Services.IServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IBaseService<PostProductVM, PostProductVM, GetProductsVM, Product> baseService) : ControllerBase
    {
        private readonly IBaseService<PostProductVM, PostProductVM, GetProductsVM, Product> _baseService = baseService;
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            IEnumerable<GetProductsVM> result = await _baseService.GetAsync();
            return Ok(new BaseResponseModel<IEnumerable<GetProductsVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }
        [HttpPost]
        public async Task<IActionResult> PostProduct(PostProductVM model)
        {
            await _baseService.PostAsync(model);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Thêm sản phẩm mới thành công"));
        }
    }
}
