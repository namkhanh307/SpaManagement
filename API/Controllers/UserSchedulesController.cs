using Core.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Repos.Entities;
using Repos.ViewModels.ScheduleVM;
using Repos.ViewModels;
using Repos.ViewModels.UserScheduleVM;
using Services.IServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSchedulesController: ControllerBase
    {
        private readonly IBaseService<PostUserScheduleVM, PostUserScheduleVM, GetUserScheduleVM, UserSchedule> _baseService;
        public UserSchedulesController(IBaseService<PostUserScheduleVM, PostUserScheduleVM, GetUserScheduleVM, UserSchedule> baseService)
        {
            _baseService = baseService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetUserSchedule(int pageNumber = 1, int pageSize = 10)
        {
            PagingVM<GetUserScheduleVM> result = await _baseService.GetAsync(null, null, null, pageNumber, pageSize);
            return Ok(new BaseResponseModel<PagingVM<GetUserScheduleVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }

        [HttpPost("post")]
        public async Task<IActionResult> PostUserSchedule(PostUserScheduleVM model)
        {
            await _baseService.PostAsync(model, null);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Thêm UserSchedule thành công"));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserSchedule(string userScheduleId)
        {
            await _baseService.DeleteAsync(userScheduleId);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Xoá UserSchedule thành công"));
        }
    }
}
