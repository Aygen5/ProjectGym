using System;
using System.IO.Compression;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Domain.Entites;
using ProjectGym.Domain.Entities;
using ProjectGym.Domain.Enums;
using ProjectGym.Domain.Interfaces;
using ProjectGym.Infrastructure.Data;

namespace ProjectGym.Infrastructure.Repositories;

public class MembershipRepository : Repository<Membership>, IMembershipRepository
{
    public MembershipRepository(ProjectGymDbContext context) : base(context)
    {
    }


    public async Task<Membership?> GetActiveByMemberIdAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;

        return await _dbSet
            .Include(m=>m.MembershipPlan)
            .FirstOrDefaultAsync(  
                m=>  m.MemberId==memberId && 
                     m.Status==MembershipStatus.Active &&
                     m.StartDate<=now && m.EndDate >= now,
                     cancellationToken
            );
    }

    public async Task<bool> HasOverlappingActiveMembershipAsync(int memberId, DateTime startDate, DateTime endDate, int? excludeMembershipId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(m=>
            m.MemberId==memberId && m.Status==MembershipStatus.Active);

        if(excludeMembershipId.HasValue)
           query = query.Where(m=>m.Id != excludeMembershipId.Value);

        return await query.AnyAsync(   
            m=>startDate<=m.EndDate && endDate>=m.StartDate, cancellationToken);
        
    }
}