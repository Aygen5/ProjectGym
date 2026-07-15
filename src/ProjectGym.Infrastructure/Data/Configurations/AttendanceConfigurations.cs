using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectGym.Domain.Entities;

namespace ProjectGym.Infrastructure.Data.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.ToTable("Attendances");

        builder.HasKey(a=>a.Id);

        builder.Property(a=>a.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasIndex(a=> new {a.MemberId, a.WorkoutSessionId});

        builder.HasIndex(a=>a.WorkoutSessionId);



        builder.HasOne(a=>a.Member)
            .WithMany(m=>m.Attendances)
            .HasForeignKey(a=>a.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a=>a.WorkoutSession)
            .WithMany(ws=>ws.Attendances)
            .HasForeignKey(a=>a.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
