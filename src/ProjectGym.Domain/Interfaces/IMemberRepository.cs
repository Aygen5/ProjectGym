using System;
using ProjectGym.Domain.Entities;

namespace ProjectGym.Domain.Interfaces;

public interface IMemberRepository : IRepository<Member>
{
    Task<Member?>GetByUserIdAsync(string userId, CancellationToken cancellationToken=default);

    Task<(IEnumerable<Member> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        bool? isActive,
        string? searchTerm,
        CancellationToken cancellationToken = default
    );

    
}