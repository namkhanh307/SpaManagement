using API.Middleware;
using Core.Infrastructures;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NhaMayMay.API.DI;
using NhaMayMay.Services;
using Repos.DbContextFactory;
using Repos.Entities;
using System.Text;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new TimeOnlySwaggerConverter());
                }); ;
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //DI API
            builder.Services.AddInfrastructure(builder.Configuration);
            //DI Services
            builder.Services.AddApplication(builder.Configuration);
            
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            var app = builder.Build();
            Authentication.SecretKey = builder.Configuration["JWT:SecretKey"] ?? "";
            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            //Add Middleware
            app.UseMiddleware<RefreshTokenMiddleware>();
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();
            app.UseMiddleware<PermissionHandlingMiddleware>();
            app.MapControllers();
            app.Run();
        }
    }
}
