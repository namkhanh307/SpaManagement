using Core.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Repos.Entities;
using Repos.ViewModels;
using Repos.ViewModels.ServiceVM;
using Services.IServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController(IBaseService<PostServiceVM, PutServiceVM, GetServicesVM,Service> baseService) : ControllerBase
    {
        private readonly IBaseService<PostServiceVM , PutServiceVM, GetServicesVM, Service> _baseService = baseService;
        [HttpGet]
        public async Task<IActionResult> GetService(int pageNumber = 1, int pageSize = 10)
        {
            PagingVM<GetServicesVM> result = await _baseService.GetAsync(null, null, null, pageNumber, pageSize);
            return Ok(new BaseResponseModel<PagingVM<GetServicesVM>>(
              statusCode: StatusCodes.Status200OK,
              code: ResponseCodeConstants.SUCCESS,
              data: result));
        }
        [HttpPost]
        public async Task<IActionResult> PostService(PostServiceVM model)
        {
            await _baseService.PostAsync(model);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Thêm dịch  mới thành công"));
        }
        [HttpPut]
        public async Task<IActionResult> PutService(string id, PutServiceVM model)
        {
            await _baseService.PutAsync(id, model);
            return Ok(new BaseResponseModel<string>(
               statusCode: StatusCodes.Status200OK,
               code: ResponseCodeConstants.SUCCESS,
               data: "Sửa dịch vụ mới thành công"));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteService(string id)
        {
            await _baseService.DeleteAsync(id);
            return Ok(new BaseResponseModel<string>(
              statusCode: StatusCodes.Status200OK,
              code: ResponseCodeConstants.SUCCESS,
              data: "Xóa dịch vụ thành công"));
        }
    }
}
