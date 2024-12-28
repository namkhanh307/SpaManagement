using Core.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repos.Entities;
using Repos.ViewModels;
using Repos.ViewModels.OrderVM;
using Services.IServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IBaseService<PostOrderVM, PostOrderVM, GetOrdersVM, Order> _baseService;
        public OrdersController(IBaseService<PostOrderVM, PostOrderVM, GetOrdersVM, Order> baseService)
        {
            _baseService = baseService;
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder(PostOrderVM model)
        {
            await _baseService.PostAsync(model);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Thêm order mới thành công"));
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders(int pageNumber = 1, int pageSize = 10, string? userId = null)
        {
            PagingVM<GetOrdersVM> result = await _baseService.GetAsync(o => o.Include(a => a.User).Include(a => a.OrderDetails), o => o.UserId == userId, o => o.OrderBy(opt => opt.CreatedAt), pageNumber, pageSize);
            return Ok(new BaseResponseModel<PagingVM<GetOrdersVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(string orderId)
        {
            await _baseService.DeleteAsync(orderId);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Xoá order thành công"));
        }
    }
}
