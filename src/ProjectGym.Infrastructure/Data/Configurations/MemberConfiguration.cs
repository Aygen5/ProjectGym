using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectGym.Domain.Entities;
using ProjectGym.Infrastructure.Identity;


namespace ProjectGym.Infrastructure.Data.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>

{
     public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");

        builder.HasKey(m=>m.Id);

        builder.Property(m=>m.UserId).IsRequired().HasMaxLength(450);

        builder.Property(m=>m.FirstName).IsRequired().HasMaxLength(50);

        builder.Property(m=>m.LastName).IsRequired().HasMaxLength(50);

        builder.Property(m=>m.Email).IsRequired().HasMaxLength(256);

        builder.Property(m=>m.JoinDate).HasDefaultValueSql("NOW()");

        builder.HasIndex(m=>m.UserId).IsUnique();

        builder.HasIndex(m=>m.Email).IsUnique();



        builder.HasOne<AppIdentityUser>()
             .WithMany()
             .HasForeignKey(m=>m.UserId)
             .OnDelete(DeleteBehavior.Restrict);



        builder.HasMany(m=>m.Memberships)
            .WithOne(ms=>ms.Member)
            .HasForeignKey(ms=>ms.MemberId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasMany(m=>m.Attendances)
            .WithOne(a=>a.Member)
            .HasForeignKey(a=>a.MemberId)
            .OnDelete(DeleteBehavior.Restrict);


    }
}