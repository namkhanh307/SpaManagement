using Core.Infrastructures;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repos.DbContextFactory;
using Repos.IRepos;
using Repos.Repos;
using System.Text;

namespace API.DI
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigSwagger();
            services.AddAuthenJwt(configuration);
            services.AddDatabase(configuration);
            services.AddLoggings();
            //services.ConfigRoute();
            services.ConfigCors();
            //services.ConfigCorsSignalR();
            //services.RabbitMQConfig(configuration);
            services.JWTConfig(configuration);
            services.IntSeedData();
        }
        public static void JWTConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(option =>
            {
                JWT JWT = new JWT
                {
                    SecretKey = configuration.GetValue<string>("JWT:SecretKey"),
                    Issuer = configuration.GetValue<string>("JWT:Issuer"),
                    Audience = configuration.GetValue<string>("JWT:Audience"),
                    AccessTokenExpirationMinutes = configuration.GetValue<int>("JWT:AccessTokenExpirationMinutes"),
                    RefreshTokenExpirationDays = configuration.GetValue<int>("JWT:RefreshTokenExpirationDays")
                };
                JWT.IsValid();
                return JWT;
            });
        }
        //public static void RabbitMQConfig(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddSingleton(option =>
        //    {
        //        RabbitMQSettings settings = new()
        //        {
        //            HostName = configuration.GetValue<string>("RabbitMQ:HostName"),
        //            UserName = configuration.GetValue<string>("RabbitMQ:UserName"),
        //            Password = configuration.GetValue<string>("RabbitMQ:Password"),
        //            Port = configuration.GetValue<int>("RabbitMQ:Port"),
        //            QueueChannel = new QueueChannel
        //            {
        //                QaQueue = configuration.GetValue<string>("RabbitMQ:QueueChannel:QaQueue"),
        //                TaskProductQueue = configuration.GetValue<string>("RabbitMQ:QueueChannel:TaskProductQueue"),
        //            }
        //        };
        //        settings.IsValid();
        //        return settings;
        //    });
        //}
        public static void ConfigCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("*")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
        }
        //public static void ConfigCorsSignalR(this IServiceCollection services)
        //{
        //    services.AddCors(options =>
        //    {
        //        options.AddPolicy("AllowSpecificOrigin",
        //            builder =>
        //            {
        //                builder.WithOrigins("https://localhost:7016")
        //                       .AllowAnyHeader()
        //                       .AllowAnyMethod()
        //                       .AllowCredentials();
        //            });
        //    });
        //}
        public static void ConfigRoute(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });
        }
        public static void AddAuthenJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var JWT = configuration.GetSection("JWT");
            services.AddAuthentication(e =>
            {
                e.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                e.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(e =>
            {
                e.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = JWT["Issuer"],
                    ValidAudience = JWT["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWT["SecretKey"]))

                };
                e.SaveToken = true;
                e.RequireHttpsMetadata = true;
                e.Events = new JwtBearerEvents();
            });
        }
        public static void ConfigSwagger(this IServiceCollection services)
        {
            // config swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "API"

                });
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
                // Thêm JWT Bearer Token vào Swagger
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "JWT Authorization header sử dụng scheme Bearer.",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Name = "Authorization",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                // Đọc các nhận xét XML
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
            });

        }
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SpaManagementContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }
        public static void AddLoggings(this IServiceCollection services)
        {
            services.AddLogging();
        }
        public static void IntSeedData(this IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<SpaManagementContext>();
            var initialiser = new SeedData(context);
            initialiser.Initialise();
        }
    }
}
