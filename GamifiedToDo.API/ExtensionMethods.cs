using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using GamifiedToDo.Adapters.Data;
using GamifiedToDo.Adapters.Data.Models;
using GamifiedToDo.Adapters.Data.Repositories;
using GamifiedToDo.API.Validators;
using GamifiedToDo.Services.App.Boards;
using GamifiedToDo.Services.App.Chores;
using GamifiedToDo.Services.App.Dep.Boards;
using GamifiedToDo.Services.App.Dep.Chores;
using GamifiedToDo.Services.App.Dep.UserLevels;
using GamifiedToDo.Services.App.Dep.Users;
using GamifiedToDo.Services.App.Int.Boards;
using GamifiedToDo.Services.App.Int.Chores;
using GamifiedToDo.Services.App.Int.UserLevels;
using GamifiedToDo.Services.App.Int.Users;
using GamifiedToDo.Services.App.UserLevels;
using GamifiedToDo.Services.App.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GamifiedToDo.API;

public static class ExtensionMethods
{
    public static void RegisterOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JsonWebTokenSettings>(configuration.GetSection(nameof(JsonWebTokenSettings)));
    }

    public static void RegisterComponents(this IServiceCollection services)
    {
        services
            .AddScoped<IChoreService, ChoreService>()
            .AddScoped<IChoreRepository, ChoreRepository>()
            .AddScoped<IUserLevelService, UserLevelService>()
            .AddScoped<IUserLevelRepository, UserLevelRepository>();

        services
            .AddScoped<IUserService, UserService>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IJsonWebTokenProvider, JsonWebTokenProvider>();

        services
            .AddScoped<IBoardService, BoardService>()
            .AddScoped<IBoardRepository, BoardRepository>();
    }

    public static void RegisterExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DataContext")));
    }

    public static void RegisterSecurityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JsonWebTokenSettings:Issuer"],
                    ValidAudience = configuration["JsonWebTokenSettings:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["JsonWebTokenSettings:Key"] ?? string.Empty))
                };
            });
    }

    public static void AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ValidatorsNamespace>();
        services.AddFluentValidationAutoValidation();
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gamified to do", Version = "v1" });
            var openApiSecurityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference()
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            c.AddSecurityDefinition(openApiSecurityScheme.Reference.Id, openApiSecurityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { openApiSecurityScheme, new List<string>() }
            });
        });
    }
}