using Microsoft.AspNetCore.Identity;
using ProjectGym.API.Extensions;
using ProjectGym.API.Filters;
using ProjectGym.Infrastructure.Data;
using ProjectGym.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiResponseFilter>();
});
builder.Services.AddOpenApi();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
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
