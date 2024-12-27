using Core.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Repos.Entities;
using Repos.ViewModels.ProductVM;
using Repos.ViewModels.ServiceVM;
using Services.IServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController(IBaseService<PostServicesVM, PutServicesVM, GetServicesVM,Service> baseService) : ControllerBase
    {
        private readonly IBaseService<PostServicesVM , PutServicesVM, GetServicesVM, Service> _baseService = baseService;
        [HttpGet]
        public async Task<IActionResult> GetService()
        {
            IEnumerable<GetServicesVM> result = await _baseService.GetAsync();
            return Ok(new BaseResponseModel<IEnumerable<GetServicesVM>>(
              statusCode: StatusCodes.Status200OK,
              code: ResponseCodeConstants.SUCCESS,
              data: result));
        }
        [HttpPost]
        public async Task<IActionResult> PostSerice(PostServicesVM model)
        {
            await _baseService.PostAsync(model);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Thêm dịch  mới thành công"));
        }
        [HttpPut]
        public async Task<IActionResult> PutService(string id, PutServicesVM model)
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
