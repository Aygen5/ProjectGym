using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticAssets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectGym.Application.DTOs.Common;
using ProjectGym.Application.Interfaces;
using ProjectGym.Application.Mappings;
using ProjectGym.Application.Services;
using ProjectGym.Domain.Interface;
using ProjectGym.Domain.Interfaces;
using ProjectGym.Infrastructure.Data;
using ProjectGym.Infrastructure.Identity;
using ProjectGym.Infrastructure.Repositories;
using ProjectGym.Infrastructure.Services;

namespace ProjectGym.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services,
    IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
        throw  new InvalidOperationException("Connection string(Default Connection)bulunamadı!");

        services.AddDbContext<ProjectGymDbContext>(options=>options.UseNpgsql (connectionString));

        return services;
    }

    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentity<AppIdentityUser, IdentityRole>(Options=>
        {
            Options.Password.RequireDigit=true;
            Options.Password.RequiredLength=8;
            Options.Password.RequireNonAlphanumeric=true;
            Options.Password.RequireUppercase=true;
            Options.Password.RequireLowercase=true;
            Options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(15);
            Options.Lockout.MaxFailedAccessAttempts=5;
            Options.User.RequireUniqueEmail=true;   
        })
        .AddEntityFrameworkStores<ProjectGymDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IAuthService, AuthService>();
        return services;
    }


    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IMembershipPlanRepository, MembershipPlanRepository>();
        services.AddScoped<IMembershipRepository, MembershipRepository>();
        services.AddScoped<ITrainerRepository, TrainerRepository>();
        services.AddScoped<IWorkoutSessionRepository, WorkoutSessionRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

   public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMembershipPlanService, MembershipPlanService>();
        services.AddScoped<ITrainerService, TrainerService>();

        services.AddAutoMapper(cfg=>cfg.AddProfile<MappingProfile>());
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JwtSettings:SecretKey bulunamadı.");

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
                options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer=true,
                    ValidateAudience=true,
                    ValidateLifetime=true,
                    ValidateIssuerSigningKey=true,
                    ValidIssuer=jwtSettings["Issuer"],
                    ValidAudience=jwtSettings["Auidence"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew=TimeSpan.Zero  
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var response = ApiResponseFactory.Failure("Kimlik doğrulama gereklidir.");
                        await context.Response.WriteAsJsonAsync(response);
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode=403;
                        context.Response.ContentType="application/json";
                        var response = ApiResponseFactory.Failure("Bu işlem için yetkiniz yok.");
                        await context.Response.WriteAsJsonAsync(response);
                    }
                };
            });
        
        return services;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy=> 
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            options.AddPolicy("AllowSpecific", policy=>
                policy.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
        });
        return services;
    }
}



