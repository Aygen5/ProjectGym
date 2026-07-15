using System;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Domain.Entities;
using ProjectGym.Domain.Interfaces;
using ProjectGym.Infrastructure.Data;

namespace ProjectGym.Infrastructure.Repositories;

public class MemberRepository : Repository<Member>, IMemberRepository
{
    public MemberRepository(ProjectGymDbContext context) : base(context)
    {
    }

    public async Task<Member?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)=>await _dbSet.FirstOrDefaultAsync(m=>m.UserId==userId, cancellationToken);

 
    public async Task<(IEnumerable<Member> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, bool? isActive, string? searchTerm, CancellationToken cancellationToken = default)
    {  
    var query = _dbSet.AsNoTracking().AsQueryable();

        if(isActive.HasValue) query = query.Where(m=>m.IsActive == isActive.Value);

        if(!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim();
            query = query.Where( m=>m.FirstName.Contains(term) || m.LastName.Contains(term) || m.Email.Contains(term));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(m=>m.LastName)
            .ThenBy(m=>m.FirstName)
            .Skip((pageNumber-1)*pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return (items, totalCount);
    }
}