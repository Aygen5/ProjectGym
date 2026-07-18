using Microsoft.AspNetCore.Identity;
using ProjectGym.API.Extensions;
using ProjectGym.Infrastructure.Data;
using ProjectGym.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddIdentityServices();
builder.Services.AddRepositories();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapOpenApi();
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<ProjectGymDbContext>();
        var userManager = services.GetRequiredService<UserManager<AppIdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await DbInitializer.SeedAsync(context,userManager,roleManager,logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"Seed işlemi sırasında bir hata oluştu: {ex.Message}");
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();