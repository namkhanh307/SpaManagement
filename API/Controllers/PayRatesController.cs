using Core.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repos.Entities;
using Repos.ViewModels;
using Repos.ViewModels.PayRateVM;
using Services.IServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayRatesController(IBaseService<PostPayRateVM, PostPayRateVM, GetPayRatesVM, PayRate> baseService) : ControllerBase
    {

        private readonly IBaseService<PostPayRateVM, PostPayRateVM, GetPayRatesVM, PayRate> _baseService = baseService;
        [HttpGet("get")]
        public async Task<IActionResult> GetPayRates(int pageNumber = 1, int pageSize = 10, string? userId = null, bool? order = true)
        {
            PagingVM<GetPayRatesVM> result = await _baseService.GetAsync(include: o => o.Include(r => r.User), filter: o => o.UserId == userId || string.IsNullOrWhiteSpace(userId), orderBy: order == true ? o => o.OrderBy(r => r.Amount) : o => o.OrderByDescending(r => r.Amount), pageNumber, pageSize);
            return Ok(new BaseResponseModel<PagingVM<GetPayRatesVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }
        [HttpPost("post")]
        public async Task<IActionResult> PostPayRate(PostPayRateVM model)
        {
            await _baseService.PostAsync(model);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Thêm luơng theo giờ mới thành công"));
        }
        [HttpPut]
        public async Task<IActionResult> PutPayRate(string id, PostPayRateVM model)
        {
            await _baseService.PutAsync(id, model);
            return Ok(new BaseResponseModel<string>(
               statusCode: StatusCodes.Status200OK,
               code: ResponseCodeConstants.SUCCESS,
               data: "Sửa luơng theo giờ thành công"));
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePayRate(string id)
        {
            await _baseService.DeleteAsync(id);
            return Ok(new BaseResponseModel<string>(
              statusCode: StatusCodes.Status200OK,
              code: ResponseCodeConstants.SUCCESS,
              data: "Xóa luơng theo giờ thành công"));
        }
    }
}
