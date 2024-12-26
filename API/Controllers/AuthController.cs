using Core.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Repos.ViewModels.AuthVM;
using Repos.ViewModels.UserVM;
using Services.IServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService, ITokenService tokenService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(PostSignUpVM model)
        {
            await _authService.SignUp(model);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Đăng ký thành công"));
        }
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(PostSignInVM model)
        {
            GetSignInVM result = await _authService.SignIn(model);
            return Ok(new BaseResponseModel<GetSignInVM>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }
        [HttpGet("GetInfo")]
        public async Task<IActionResult> GetInfo()
        {
            GetUsersVM result = await _authService.GetInfo();
            return Ok(new BaseResponseModel<GetUsersVM>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            await _authService.ChangePassword(model);
            return Ok(new BaseResponseModel<string>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: "Đổi mật khẩu thành công!"));
        }
    }
}
