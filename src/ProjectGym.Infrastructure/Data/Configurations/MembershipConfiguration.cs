using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectGym.Domain.Entites;

namespace ProjectGym.Infrastructure.Data.Configurations;

public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.ToTable("Memberships");

        builder.HasKey(m=>m.Id);

        builder.Property(m=>m.Status)
            .HasConversion<string>()
            .HasMaxLength(50);


        builder.HasIndex(m=>m.MemberId);


        builder.HasIndex(m=> new{m.MemberId, m.Status});


        builder.HasOne(m=>m.Member)
            .WithMany(mb=>mb.Memberships)
            .HasForeignKey(m=>m.MemberId)
            .OnDelete(DeleteBehavior.Restrict);



        builder.HasOne(m=>m.MembershipPlan)
            .WithMany(p=>p.Memberships)
            .HasForeignKey(m=>m.MembershipPlanId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}   