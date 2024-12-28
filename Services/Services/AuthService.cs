using AutoMapper;
using Azure.Core;
using Core.Infrastructures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repos.Entities;
using Repos.IRepos;
using Repos.ViewModels.AuthVM;
using Repos.ViewModels.UserVM;
using Services.IServices;

namespace Services.Services
{
    public class AuthService(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, UserManager<User> userManager) : IAuthService
    {
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenService _tokenService = tokenService;
        private readonly UserManager<User> _userManager = userManager;

        public async Task ChangePassword(ChangePasswordVM model)
        {
            string idUser = Authentication.GetUserIdFromHttpContextAccessor(_contextAccessor);
            User? user = await _unitOfWork.GetRepo<User>().Entities.FirstOrDefaultAsync(p => p.Id.ToString() == idUser) ?? throw new ErrorException(StatusCodes.Status401Unauthorized, ErrorCode.UnAuthorized, "Người dùng không tồn tại!");

            if (user.PasswordHash != HashPasswordService.HashPasswordThrice(model.OldPassword))
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ErrorCode.UnAuthorized, "Mật khẩu cũ không chính xác!");
            }
            if (model.NewPassword != model.ConfirmPassword)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Mật khẩu và xác nhận mật khẩu không khớp!");
            }
            user.PasswordHash = HashPasswordService.HashPasswordThrice(model.NewPassword);
            await _unitOfWork.GetRepo<User>().Update(user);
            await _unitOfWork.Save();
        }

        public async Task<GetUsersVM> GetInfo()
        {
            string idUser = Authentication.GetUserIdFromHttpContextAccessor(_contextAccessor);
            return _mapper.Map<GetUsersVM>(await _unitOfWork.GetRepo<User>().GetById(idUser));
        }

        public async Task<GetSignInVM> SignIn(PostSignInVM request)
        {
            User? user = await _unitOfWork.GetRepo<User>().Entities.FirstOrDefaultAsync(p => p.PhoneNumber == request.PhoneNumber) ?? throw new ErrorException(StatusCodes.Status401Unauthorized, ErrorCode.UnAuthorized, "Số điện thoại này chưa có tài khoản! Vui lòng đăng ký!");
            if (user.PasswordHash != HashPasswordService.HashPasswordThrice(request.Password))
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ErrorCode.UnAuthorized, "Số điện thoại hoặc mật khẩu không đúng!");
            }
            GetTokensVM token = await _tokenService.GenerateTokens(user.Id.ToString(), null);
            return new GetSignInVM()
            {
                User = _mapper.Map<GetUsersVM>(user),
                Token = token
            };
        }

        public async Task SignUp(PostSignUpVM model)
        {
            User? user = await _unitOfWork.GetRepo<User>().Entities.FirstOrDefaultAsync(p => p.PhoneNumber == model.PhoneNumber);
            if (user != null)
            {
                throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Số điện thoại này đã được đăng ký! Vui lòng đăng nhập!");
            }
            if (model.Password != model.ConfirmedPassword)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.InvalidInput, "Mật khẩu và xác nhận mật khẩu không khớp!");
            }
            User newUser = new()
            {
                Id = Guid.NewGuid().ToString("N"),
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                PasswordHash = HashPasswordService.HashPasswordThrice(model.Password),
                SecurityStamp = Guid.NewGuid().ToString("N")
            };
            await _unitOfWork.GetRepo<User>().Insert(newUser);
            await _userManager.AddToRoleAsync(newUser, "User");
            await _unitOfWork.Save();
        }
    }
}
