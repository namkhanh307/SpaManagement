using Core.Infrastructures;
using Repos.IRepos;
using System.Net;
using System.Text.Json;

namespace API.Middleware
{
    public class PermissionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PermissionHandlingMiddleware> _logger;
        private readonly IEnumerable<string> _excludedUris;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly Dictionary<string, List<string>> _rolePermissions;


        public PermissionHandlingMiddleware(RequestDelegate next, ILogger<PermissionHandlingMiddleware> logger, IHttpContextAccessor contextAccessor)
        {
            _next = next;
            _logger = logger;
            _excludedUris = new List<string>()
            {
                "/api/Auth/SignIn",
                "/api/Auth/SignUp",
                "api/Auth/ChangePassword"
            };
            _contextAccessor = contextAccessor;
            _rolePermissions = new Dictionary<string, List<string>>()
            {
                { "Staff", new List<string> {"/api/Products/", "/api/Orders" } },
                { "User", new List<string> { "/api/Products/Get" } }
            };
        }

        public async Task Invoke(HttpContext context, IUnitOfWork unitOfWork)
        {
            if (HasPermission(context))
            {
                await _next(context);
            }
            else
            {
                var code = HttpStatusCode.Forbidden;
                var result = JsonSerializer.Serialize(new { error = "Bạn không có quyền truy cập chức năng này!" });
                context.Response.ContentType = "application/json";
                context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                context.Response.StatusCode = (int)code;
                await context.Response.WriteAsync(result);
            }
        }

        private bool HasPermission(HttpContext context)
        {
            string requestUri = context.Request.Path.Value ?? "";
            if (_excludedUris.Contains(requestUri) || !requestUri.StartsWith("/api/")) return true;
            try
            {
                {
                    string userRole = Authentication.GetUserRoleFromHttpContext(context);
                    if (userRole == "Manager") return true;
                    if (_rolePermissions.TryGetValue(userRole, out var allowedControllers))
                    {
                        return allowedControllers.Any(uri => requestUri.StartsWith(uri, System.StringComparison.OrdinalIgnoreCase));
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while checking permissions");
                return false;
            }
        }
    }
}
