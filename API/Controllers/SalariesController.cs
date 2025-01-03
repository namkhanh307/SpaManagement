using Core.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Repos.Entities;
using Repos.ViewModels;
using Repos.ViewModels.SalaryVM;
using Services.IServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalariesController(IBaseService<PostSalaryVM, PostSalaryVM, GetSalariesVM, Salary> baseService) : ControllerBase
    {
        private readonly IBaseService<PostSalaryVM, PostSalaryVM, GetSalariesVM, Salary> _baseService = baseService;
        [HttpGet("get")]
        public async Task<IActionResult> GetSalaries(int pageNumber = 1, int pageSize = 10)
        {
            PagingVM<GetSalariesVM> result = await _baseService.GetAsync(null, null, null, pageNumber, pageSize);
            return Ok(new BaseResponseModel<PagingVM<GetSalariesVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }
        [HttpPost("post")]
        public async Task<IActionResult> PostSalary(PostSalaryVM model)
        {
            await _baseService.PostAsync(model);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Thêm sản phẩm mới thành công"));
        }
        [HttpPut]
        public async Task<IActionResult> PutSalary(string id, PostSalaryVM model)
        {
            await _baseService.PutAsync(id, model);
            return Ok(new BaseResponseModel<string>(
               statusCode: StatusCodes.Status200OK,
               code: ResponseCodeConstants.SUCCESS,
               data: "Sửa sản phẩm mới thành công"));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteSalary(string id)
        {
            await _baseService.DeleteAsync(id);
            return Ok(new BaseResponseModel<string>(
              statusCode: StatusCodes.Status200OK,
              code: ResponseCodeConstants.SUCCESS,
              data: "Xóa sản phẩm thành công"));
        }
    }
}
