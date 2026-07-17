using System;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectGym.Domain.Entities;
using ProjectGym.Domain.Interface;
using ProjectGym.Domain.Interfaces;
using ProjectGym.Infrastructure.Repositories;

namespace ProjectGym.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{

    private readonly ProjectGymDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ProjectGymDbContext context)
    {
        _context = context;
        Members = new MemberRepository(_context);
        MembershipPlans = new MembershipPlanRepository(_context);
        Memberships= new MembershipRepository(_context);
        Trainers = new TrainerRepository(_context);
        WorkoutSessions= new WorkoutSessionRepository(_context);
        Attendances = new AttendanceRepository(_context);
    }
    public IMemberRepository Members {get;}

    public IMembershipPlanRepository MembershipPlans {get;}

    public IMembershipRepository Memberships {get;}

    public ITrainerRepository Trainers {get;}

    public IWorkoutSessionRepository WorkoutSessions {get;}

    public IAttendanceRepository Attendances {get;}

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    =>_transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
   {
       await _transaction!.CommitAsync(cancellationToken);
       await _transaction.DisposeAsync();
       _transaction=null;

   }

    public void Dispose()=>_context.Dispose();

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _transaction!.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction=null;
    }

    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    =>_context.SaveChangesAsync(cancellationToken);

}