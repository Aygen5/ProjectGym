using System;

using ProjectGym.Domain.Entites;

namespace ProjectGym.Domain.Interfaces;

public interface IWorkoutSessionRepository : IRepository<WorkoutSession>
{

    Task<WorkoutSession?>GetWithTrainerAndAttendancesAsync(int id, CancellationToken cancellationToken=default);

    Task<(IEnumerable<WorkoutSession> Items, int TotalCount)> GetUpcomingPagedAsync(
        int pageNumber,
        int pageSize,
        DateTime? fromUtc = null,
        CancellationToken cancellationToken=default

    );

    
}