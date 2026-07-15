using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectGym.Domain.Entities;

namespace ProjectGym.Infrastructure.Data.Configurations;

public class MembershipPlanConfiguration : IEntityTypeConfiguration<MembershipPlan>
{
    public void Configure(EntityTypeBuilder<MembershipPlan> builder)
    {
        builder.ToTable("MembershipPlans");

        builder.HasKey(p=>p.Id);

        builder.Property(p=>p.Name).IsRequired().HasMaxLength(100);

        builder.Property(p=>p.PricePerMonth).HasColumnType("decimal(18,2)");

        builder.HasIndex(p=>p.Name);


        builder.HasMany(p=>p.Memberships)
            .WithOne(m=>m.MembershipPlan)
            .HasForeignKey(m=>m.MembershipPlanId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}