using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repos.IRepos;
using Repos.Repos;
using Services.IServices;
using Services.Mappers;
using Services.Services;
using System.Reflection;

namespace NhaMayMay.Services
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices(configuration);
            services.AddRepository();
            services.AddAutoMapper();
        }
        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(ProductProfile).Assembly);
            services.AddAutoMapper(typeof(OrderDetailProfile).Assembly);
            services.AddAutoMapper(typeof(OrderProfile).Assembly);
            services.AddAutoMapper(typeof(ScheduleProfile).Assembly);
            services.AddAutoMapper(typeof(UserProfile).Assembly);
            services.AddAutoMapper(typeof(ServiceProfile).Assembly);
        }
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IBaseService<,,,>), typeof(BaseService<,,,>));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
