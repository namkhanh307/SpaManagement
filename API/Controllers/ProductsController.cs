using Core.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repos.Entities;
using Repos.ViewModels;
using Repos.ViewModels.ProductVM;
using Services.IServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IBaseService<PostProductVM, PostProductVM, GetProductsVM, Product> baseService) : ControllerBase
    {
        private readonly IBaseService<PostProductVM, PostProductVM, GetProductsVM, Product> _baseService = baseService;
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
    }
}
