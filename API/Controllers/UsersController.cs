using Core.Infrastructures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repos.Entities;
using Repos.ViewModels.ProductVM;
using Repos.ViewModels;
using Services.IServices;
using Repos.ViewModels.UserVM;
using Repos.ViewModels.AuthVM;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IAuthService authService, IBaseService<PostUserVM, PostUserVM, GetUsersVM, User> baseService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly IBaseService<PostUserVM, PostUserVM, GetUsersVM, User> _baseService = baseService;
        [HttpGet("get")]
        public async Task<IActionResult> GetUsers(int pageNumber = 1, int pageSize = 10)
        {
            PagingVM<GetUsersVM> result = await _baseService.GetAsync(null, null, null, pageNumber, pageSize);
            return Ok(new BaseResponseModel<PagingVM<GetUsersVM>>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }
        [HttpPost("post")]
        public async Task<IActionResult> PostUsers(PostSignUpVM model, string role)
        {
            await _authService.SignUp(model, role);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Thêm nhân viên mới thành công"));
        }
    }
}
