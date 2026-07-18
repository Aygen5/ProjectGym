using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectGym.Domain.Entites;
using ProjectGym.Domain.Entities;
using ProjectGym.Infrastructure.Identity;

namespace ProjectGym.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(ProjectGymDbContext context, UserManager<AppIdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger logger)
    {
        await context.Database.MigrateAsync();
        await SeedRolesAsync(roleManager, logger);
        await SeedAdminUserAsync(userManager, logger);
        await SeedMembershipPlansAsync(context,logger);
        await SeedTrainersAsync(context,logger);
        await SeedWorkoutSessionsAsync(context, logger);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
    {
        string[] roles = ["Admin","Member"];
        foreach (var role in roles)
        {
            if(!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                logger.LogInformation($"Rol oluşturuldu: {role}");
            }
        }
    }

    private static async Task SeedAdminUserAsync(UserManager<AppIdentityUser> userManager, ILogger logger)
    {
        const string adminEmail = "admin@projectgym.local";
        if(await userManager.FindByEmailAsync(adminEmail)!=null) return;
        var admin = new AppIdentityUser
        {
            UserName=adminEmail,
            Email=adminEmail,
            FirstName="Admin",
            LastName="User",
            EmailConfirmed=true,
            CreatedAt=DateTime.UtcNow
        };
        var result = await userManager.CreateAsync(admin,"Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
            logger.LogInformation($"Admin kullanıcısı oluşturuldu: {adminEmail}");
        }
        else
        {
            logger.LogWarning($"Admin kullanıcısı oluşturulamadı: {string.Join(", ",result.Errors.Select(e=>e.Description))}");
        }
    }

    private static async Task SeedMembershipPlansAsync(ProjectGymDbContext context, ILogger logger)
    {
        if(await context.MembershipPlans.AnyAsync()) return;

        var plans = new List<MembershipPlan>
        {
            new()
            {
                Name="Standart",
                PricePerMonth=4999.99m,
                DurationMonths=1,
                IsActive=true
            },
            new()
            {
                Name="Premium",
                PricePerMonth=8999.99m,
                DurationMonths=3,
                IsActive=true
            }
        };

        await context.MembershipPlans.AddRangeAsync(plans);
        await context.SaveChangesAsync();
        logger.LogInformation("Üyelik planları seed edildi.");
    }

    private static async Task SeedTrainersAsync(ProjectGymDbContext context, ILogger logger)
    {
        if(await context.Trainers.AnyAsync()) return;

        var trainers = new List<Trainer>
        {
            new() {FirstName="Ayşen", LastName="Umay", Specialty="Pilates"},
            new() {FirstName="Kemal",LastName="Muratoğlu",Specialty="CrossFit"}
        };
        await context.Trainers.AddRangeAsync(trainers);
        await context.SaveChangesAsync();
        logger.LogInformation("Eğitmenler seed edildi.");
    }

    private static async Task SeedWorkoutSessionsAsync(ProjectGymDbContext context, ILogger logger)
    {
        if(await context.WorkoutSessions.AnyAsync()) return;

        var trainers = await context.Trainers.OrderBy(t=>t.Id).Take(2).ToListAsync();
        if(trainers.Count==0) return;

        var baseTime = DateTime.UtcNow.Date.AddDays(1).AddHours(9);

        var sessions = new List<WorkoutSession>
        {
            new()
            {
                TrainerId=trainers[0].Id,
                Name="Sabah Pilates",
                ScheduledTime=baseTime,
                Capacity=12,
                IsCancelled=false
            },
            new()
            {
                TrainerId=trainers[1].Id,
                Name="Öğle CrossFit",
                ScheduledTime=baseTime.AddHours(3),
                Capacity=15,
                IsCancelled=false
            },
            new()
            {
                TrainerId=trainers[0].Id,
                Name="Akşam Pilates",
                ScheduledTime=baseTime.AddHours(8),
                Capacity=10,
                IsCancelled=false
            }
        };
        await context.WorkoutSessions.AddRangeAsync(sessions);
        await context.SaveChangesAsync();
        logger.LogInformation("Gelecek seanslar seed edildi.");
    }
}
