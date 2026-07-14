using System;
using ProjectGym.Domain.Interface;

namespace ProjectGym.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IMemberRepository Members {get;}
    
    IMembershipPlanRepository MembershipPlans {get;}

    IMembershipRepository Memberships {get;}

    ITrainerRepository Trainers {get;}

    IWorkoutSessionRepository WorkoutSessions {get;}

    IAttendanceRepository Attendances {get;}

    


    Task<int> SaveChangesAsync(CancellationToken cancellationToken=default);

    Task BeginTransactionAsync(CancellationToken cancellationToken=default);

    Task CommitTransactionAsync(CancellationToken cancellationToken=default);

    Task RollbackTransactionAsync(CancellationToken cancellationToken=default);


}