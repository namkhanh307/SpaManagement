using Core.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Repos.Entities;
using Repos.ViewModels.ScheduleVM;
using Services.IServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly IBaseService<PostScheduleVM, PostScheduleVM, GetScheduleVM, Schedule> _baseService;
        public SchedulesController(IBaseService<PostScheduleVM, PostScheduleVM, GetScheduleVM, Schedule> baseService)
        {
            _baseService = baseService;
        }

        [HttpPost]
        public async Task<IActionResult> PostSchedule(PostScheduleVM model)
        {
            await _baseService.PostAsync(model);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Thêm schedule mới thành công"));
        }

        [HttpGet]
        public async Task<IActionResult> GetSchedules()
        {
            IEnumerable<GetScheduleVM> result = await _baseService.GetAsync();
            return Ok(new BaseResponseModel<IEnumerable<GetScheduleVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSchedule(string scheduleId)
        {
            await _baseService.DeleteAsync(scheduleId);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Xoá schedule thành công"));
        }
    }
}
