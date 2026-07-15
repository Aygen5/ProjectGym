using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectGym.Domain.Entites;
using ProjectGym.Domain.Entities;

namespace ProjectGym.Infrastructure.Data.Configurations;

public class WorkoutSessionConfiguration : IEntityTypeConfiguration<WorkoutSession>
{
    public void Configure(EntityTypeBuilder<WorkoutSession> builder)
    {
        builder.ToTable("WorkoutSessions");

        builder.HasKey(ws=>ws.Id);

        builder.Property(ws=>ws.Name).IsRequired().HasMaxLength(50);

        builder.HasIndex(ws=>ws.ScheduledTime);

        builder.HasIndex(ws=> new { ws.TrainerId, ws.ScheduledTime});


        builder.HasOne(ws=>ws.Trainer)
            .WithMany(t=>t.WorkoutSessions)
            .HasForeignKey(ws=>ws.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(ws=>ws.Attendances)
            .WithOne(a=>a.WorkoutSession)
            .HasForeignKey(a=>a.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
