using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Infrastructures
{
    public class Authentication
    {
        public static string SecretKey { get; set; } = string.Empty;// Static property to store the secret key
        public static string GetUserIdFromHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext == null || !httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ErrorCode.UnAuthorized, "Authorization header is required!");
            }

            string? authorizationHeader = httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ErrorCode.UnAuthorized, $"Invalid authorization header: {authorizationHeader}");

            }

            string jwtToken = authorizationHeader["Bearer ".Length..].Trim();

            if (!ValidateToken(jwtToken, out ClaimsPrincipal principal))
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ErrorCode.UnAuthorized, "Token validation failed!");

            }

            var idClaim = principal.Claims.FirstOrDefault(claim => claim.Type == "id");

            return idClaim?.Value ?? throw new ErrorException(StatusCodes.Status401Unauthorized, ErrorCode.UnAuthorized, "User ID claim not found in token!");
        }

        public static bool ValidateToken(string token, out ClaimsPrincipal principal)
        {
            principal = null!;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.UTF8.GetBytes(SecretKey); // Ensure UTF-8 encoding

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key), // Use the secret key to verify signature
                    ValidateIssuer = false, // Optional: set to true if you want to validate the issuer
                    ValidateAudience = false, // Optional: set to true if you want to validate the audience
                    ClockSkew = TimeSpan.Zero, // Optional: to account for small time differences
                    RequireSignedTokens = false // To avoid 'kid' requirement issues with symmetric keys
                };

                principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                return validatedToken is JwtSecurityToken jwtToken &&
                       jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Token validation failed: " + ex.Message);
                return false;
            }
        }


        private static string HandleUnauthorizedResponse(IHttpContextAccessor httpContextAccessor, string errorMessage)
        {
            var errorResponse = new
            {
                data = "An unexpected error occurred.",
                additionalData = (object)null!,
                message = errorMessage,
                statusCode = StatusCodes.Status401Unauthorized,
                code = "Unauthorized!"
            };

            var jsonResponse = System.Text.Json.JsonSerializer.Serialize(errorResponse);

            if (httpContextAccessor.HttpContext != null)
            {
                httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                httpContextAccessor.HttpContext.Response.ContentType = "application/json";
                httpContextAccessor.HttpContext.Response.WriteAsync(jsonResponse).Wait();
            }

            return null!;
        }
        public static string GetUserIdFromHttpContext(HttpContext httpContext)
        {
            try
            {
                if (!httpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    throw new UnauthorizedException("Need Authorization");
                }

                string? authorizationHeader = httpContext.Request.Headers["Authorization"];

                if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    throw new UnauthorizedException($"Invalid authorization header: {authorizationHeader}");
                }

                string jwtToken = authorizationHeader["Bearer ".Length..].Trim();

                var tokenHandler = new JwtSecurityTokenHandler();

                if (!tokenHandler.CanReadToken(jwtToken))
                {
                    throw new UnauthorizedException("Invalid token format");
                }

                var token = tokenHandler.ReadJwtToken(jwtToken);
                var idClaim = token.Claims.FirstOrDefault(claim => claim.Type == "id");

                return idClaim?.Value ?? throw new UnauthorizedException("Cannot get userId from token");
            }
            catch (UnauthorizedException ex)
            {
                var errorResponse = new
                {
                    data = "An unexpected error occurred.",
                    additionalData = (object)null!, // Explicitly setting null type
                    message = ex.Message,
                    statusCode = StatusCodes.Status401Unauthorized,
                    code = "Unauthorized!"
                };

                var jsonResponse = System.Text.Json.JsonSerializer.Serialize(errorResponse);

                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.WriteAsync(jsonResponse).Wait();

                throw; // Re-throw the exception to maintain the error flow
            }
        }

        public static string GetUserRoleFromHttpContext(HttpContext httpContext)
        {
            try
            {
                if (!httpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    throw new UnauthorizedException("Need Authorization");
                }

                string? authorizationHeader = httpContext.Request.Headers["Authorization"];

                if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    throw new UnauthorizedException($"Invalid authorization header: {authorizationHeader}");
                }

                string jwtToken = authorizationHeader["Bearer ".Length..].Trim();

                var tokenHandler = new JwtSecurityTokenHandler();

                if (!tokenHandler.CanReadToken(jwtToken))
                {
                    throw new UnauthorizedException("Invalid token format");
                }

                var token = tokenHandler.ReadJwtToken(jwtToken);
                var roleClaim = token.Claims.FirstOrDefault(claim => claim.Type == "role");

                return roleClaim?.Value ?? throw new UnauthorizedException("Cannot get user role from token");
            }
            catch (UnauthorizedException ex)
            {
                var errorResponse = new
                {
                    data = "An unexpected error occurred.",
                    additionalData = (object)null!, // Explicitly setting null type
                    message = ex.Message,
                    statusCode = StatusCodes.Status401Unauthorized,
                    code = "Unauthorized!"
                };

                var jsonResponse = System.Text.Json.JsonSerializer.Serialize(errorResponse);

                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.WriteAsync(jsonResponse).Wait();

                throw; // Re-throw the exception to maintain the error flow
            }
        }

    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }
}


