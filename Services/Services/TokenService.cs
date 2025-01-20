using Core.Infrastructures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repos.Entities;
using Repos.IRepos;
using Repos.ViewModels.AuthVM;
using Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public TokenService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _configuration = builder.Build();
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<GetTokensVM> GenerateTokens(string userId, DateTime? expiredTime)
        {
            DateTime now = DateTime.Now;
            User? user = await _unitOfWork.GetRepo<User>().GetById(userId) ?? throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.BadRequest, "Người dùng không tồn tại!");
            Task<IList<string>>? role = _userManager.GetRolesAsync(user);
            if (role.Result.Count == 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ErrorCode.BadRequest, "Vai trò không tồn tại!");
            }
            var atClaims = new List<Claim>
            {
                new("id", userId.ToString()),
                new("role", role.Result.FirstOrDefault()!),
                new("name", "AT")
            };
            var rtClaims = new List<Claim>
            {
                new("id", userId.ToString()),
                new("role", role.Result.FirstOrDefault()!),
                new("name", "RT")
            };
            int expiredMinutes = int.Parse(_configuration["JWT:AccessTokenExpirationMinutes"]!);
            int expiredDays = int.Parse(_configuration["JWT:RefreshTokenExpirationDays"]!);

            var keyString = _configuration.GetSection("JWT:SecretKey").Value ?? string.Empty;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Generate access token
            var accessToken = new JwtSecurityToken(
                issuer: _configuration.GetSection("JWT:Issuer").Value,
                audience: _configuration.GetSection("JWT:Audience").Value,
                claims: atClaims,
                notBefore: now,
                expires: now.AddMinutes(expiredMinutes),
                signingCredentials: creds
            );
            var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

            // Generate refresh token
            var refreshToken = new JwtSecurityToken(
                issuer: _configuration.GetSection("JWT:Issuer").Value,
                audience: _configuration.GetSection("JWT:Audience").Value,
                claims: rtClaims,
                notBefore: now,
                expires: expiredTime != null ? expiredTime : now.AddDays(expiredDays),
                signingCredentials: creds
            );
            var refreshTokenString = new JwtSecurityTokenHandler().WriteToken(refreshToken);
            var userTokenRepo = _unitOfWork.GetRepo<UserTokens>();
            var userToken = await userTokenRepo.Entities.FirstOrDefaultAsync(ut => ut.UserId.ToString() == userId);
            if (userToken == null)
            {
                UserTokens newUserToken = new()
                {
                    UserId = userId,
                    LoginProvider = "JWT",
                    Name = "RefreshToken",
                    RefreshToken = refreshTokenString,
                    ExpiredTime = now.AddDays(expiredDays)
                };
                await _unitOfWork.GetRepo<UserTokens>().Insert(newUserToken);
                await _unitOfWork.Save();
            }
            // Return the tokens
            return new GetTokensVM
            {
                AccessToken = accessTokenString,
                RefreshToken = refreshTokenString
            };
        }
        public async Task<GetTokensVM> RefreshToken(string oldRT)
        {
            var userId = Authentication.GetUserIdFromHttpContextAccessor(_httpContextAccessor);
            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("User not authenticated.");
            DateTime expiredTime;
            try
            {
                expiredTime = DecodeOldRefreshToken(oldRT);
            }
            catch
            {
                throw new ArgumentException("Invalid old refresh token.");
            }

            var userTokenRepo = _unitOfWork.GetRepo<UserTokens>();
            var userToken = await userTokenRepo.Entities.FirstOrDefaultAsync(ut => ut.UserId.ToString() == userId);
            GetTokensVM tokensVM = await GenerateTokens(userId, expiredTime);
            if (userToken != null)
            {
                userToken.RefreshToken = tokensVM.RefreshToken;
                userToken.ExpiredTime = expiredTime;
                await userTokenRepo.Update(userToken);
                await _unitOfWork.Save();
            }
            return new GetTokensVM
            {
                AccessToken = tokensVM.AccessToken,
                RefreshToken = tokensVM.RefreshToken
            };
        }
        private DateTime DecodeOldRefreshToken(string oldRefreshToken)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(oldRefreshToken))
                throw new ArgumentException("Invalid token format.");

            var jwtToken = handler.ReadJwtToken(oldRefreshToken);
            var expiredClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);

            if (expiredClaim == null)
                throw new ArgumentException("Token does not contain expiry information.");

            var expiredTimeUnix = long.Parse(expiredClaim.Value);
            var expiredTime = DateTimeOffset.FromUnixTimeSeconds(expiredTimeUnix).UtcDateTime;

            return expiredTime;
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!);
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false, // We allow expired tokens to validate the user
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidAudience = _configuration["JWT:Audience"]
            };
            try
            {
                var principal = tokenHandler.ValidateToken(token, parameters, out _);
                return principal;
            }
            catch
            {
                return null!;
            }
        }

        public bool IsTokenExpired(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwtToken.ValidTo < DateTime.UtcNow;
        }
    }
}
