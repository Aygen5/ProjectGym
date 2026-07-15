using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectGym.Domain.Entites;
using ProjectGym.Domain.Entities;

namespace ProjectGym.Infrastructure.Data.Configurations;

public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.ToTable("Trainers");

        builder.HasKey(static t =>t.Id);

        builder.Property(static t =>t.FirstName).IsRequired().HasMaxLength(50);

        builder.Property(static t =>t.LastName).IsRequired().HasMaxLength(50);

        builder.Property(static t =>t.Specialty).IsRequired().HasMaxLength(100);

        builder.HasMany(t=>t.WorkoutSessions)
            .WithOne(static ws =>ws.Trainer)
            .HasForeignKey(static ws =>ws.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
