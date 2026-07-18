using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticAssets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjectGym.Application.Interfaces;
using ProjectGym.Domain.Interface;
using ProjectGym.Domain.Interfaces;
using ProjectGym.Infrastructure.Data;
using ProjectGym.Infrastructure.Identity;
using ProjectGym.Infrastructure.Repositories;

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

        //services.AddScoped<IAuthService, AuthService>();
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


}

