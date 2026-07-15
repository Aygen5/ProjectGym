using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ProjectGym.Infrastructure.Data;

public class DesignTimeDbContextFactory
{
     public ProjectGymDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(),"../ProjectGym.API"))
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "buraya connection stringi yazabiliriz.";

        var optionsBuilder = new DbContextOptionsBuilder<ProjectGymDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new ProjectGymDbContext(optionsBuilder.Options);
    }
}
        

