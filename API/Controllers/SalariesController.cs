using Core.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repos.Entities;
using Repos.ViewModels;
using Repos.ViewModels.SalaryVM;
using Services.IServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalariesController(IBaseService<PostSalaryVM, PutSalaryVM, GetSalariesVM, Salary> baseService, ISalaryService salaryService) : ControllerBase
    {
        private readonly IBaseService<PostSalaryVM, PutSalaryVM, GetSalariesVM, Salary> _baseService = baseService;
        private readonly ISalaryService _salaryService = salaryService;
        [HttpGet("get")]
        public async Task<IActionResult> GetSalaries(int pageNumber = 1, int pageSize = 10, string? userId = null, int? month = null, int? year = null)
        {
            PagingVM<GetSalariesVM> result = await _baseService.GetAsync(include: o => o.Include(r => r.User), filter: o => (o.UserId == userId || string.IsNullOrWhiteSpace(userId)) && (o.Month == month || month == null) && (o.Year == year || year == null), null, pageNumber, pageSize);
            return Ok(new BaseResponseModel<PagingVM<GetSalariesVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }
        [HttpPost("post")]
        public async Task<IActionResult> PostSalary(PostSalaryVM model)
        {
            await _salaryService.PostAsync(model);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Thêm lương mới thành công"));
        }
        [HttpPut]
        public async Task<IActionResult> PutSalary(string id, PutSalaryVM model)
        {
            await _baseService.PutAsync(id, model);
            return Ok(new BaseResponseModel<string>(
               statusCode: StatusCodes.Status200OK,
               code: ResponseCodeConstants.SUCCESS,
               data: "Sửa lương thành công"));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteSalary(string id)
        {
            await _baseService.DeleteAsync(id);
            return Ok(new BaseResponseModel<string>(
              statusCode: StatusCodes.Status200OK,
              code: ResponseCodeConstants.SUCCESS,
              data: "Xóa lương thành công"));
        }
    }
}
