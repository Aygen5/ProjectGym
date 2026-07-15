using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Domain.Entites;
using ProjectGym.Domain.Entities;
using ProjectGym.Infrastructure.Identity;

namespace ProjectGym.Infrastructure.Data;

public class ProjectGymDbContext : IdentityDbContext<AppIdentityUser>
{
    public ProjectGymDbContext(DbContextOptions<ProjectGymDbContext> options) : base(options)
    {

    }

    public DbSet<Member> Members => Set<Member>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<MembershipPlan> MembershipPlans => Set<MembershipPlan>();
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<WorkoutSession> WorkoutSessions => Set<WorkoutSession>();
    public DbSet<Attendance> Attendances => Set<Attendance>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ProjectGymDbContext).Assembly);
    }
}

