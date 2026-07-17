using System;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Domain.Entites;
using ProjectGym.Domain.Entities;
using ProjectGym.Domain.Interfaces;
using ProjectGym.Infrastructure.Data;

namespace ProjectGym.Infrastructure.Repositories;

public class WorkoutSessionRepository : Repository<WorkoutSession>, IWorkoutSessionRepository
{
    public WorkoutSessionRepository(ProjectGymDbContext context) : base(context)
    {
    }

    public async Task<(IEnumerable<WorkoutSession> Items, int TotalCount)> GetUpcomingPagedAsync(int pageNumber, int pageSize, DateTime? fromUtc = null, CancellationToken cancellationToken = default)
    {
        var from = fromUtc ?? DateTime.UtcNow;

        var query = _dbSet
            .AsNoTracking()
            .Include(ws=>ws.Trainer)
            .Where(ws=>!ws.IsCancelled && ws.ScheduledTime>=from);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query 
            .OrderBy(ws=>ws.ScheduledTime)
            .Skip((pageNumber-1)*pageSize)
            .Take(pageSize) 
            .ToListAsync(cancellationToken);   

        return (items, totalCount);
    
    }

    public async Task<WorkoutSession?> GetWithTrainerAndAttendancesAsync(int id, CancellationToken cancellationToken = default)=>
         await _dbSet
             .Include(ws=>ws.Trainer)
             .Include(ws=>ws.Attendances)
             .ThenInclude(a=>a.Member)
             .FirstOrDefaultAsync(ws=>ws.Id==id, cancellationToken);
  
}