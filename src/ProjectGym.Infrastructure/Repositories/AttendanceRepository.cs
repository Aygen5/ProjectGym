using System;
using System.Net.WebSockets;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Domain.Entities;
using ProjectGym.Domain.Enums;
using ProjectGym.Domain.Interfaces;
using ProjectGym.Infrastructure.Data;

namespace ProjectGym.Infrastructure.Repositories;

public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
{
    public AttendanceRepository(ProjectGymDbContext context) : base(context)
    {
    }

    public async Task<int> CountRegisteredAsync(int WorkoutSessionId, CancellationToken cancellationToken = default)=> await _dbSet
       .CountAsync(a=>a.WorkoutSessionId==WorkoutSessionId && a.Status==AttendanceStatus.Registered, cancellationToken);

   
    public async Task<bool> ExistsRegisteredAsync(int memberId, int workoutSessionId, CancellationToken cancellationToken = default)=> await _dbSet
        .AnyAsync(a=>a.MemberId==memberId && a.WorkoutSessionId==workoutSessionId && a.Status==AttendanceStatus.Registered, cancellationToken);
 

    public async Task<IEnumerable<Attendance>> GetByMemberIdAsync(int memberId, CancellationToken cancellationToken = default)=> await _dbSet
        .AsNoTracking()
        .Include(a=>a.WorkoutSession)
        .ThenInclude(ws=>ws.Trainer)
        .Where(a=>a.MemberId==memberId)
        .OrderByDescending(a=>a.CheckInTime)
        .ToListAsync(cancellationToken);
}