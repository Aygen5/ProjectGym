using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Domain.Interfaces;
using ProjectGym.Infrastructure.Data;

namespace ProjectGym.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ProjectGymDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ProjectGymDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) => await _dbSet.AddAsync(entity, cancellationToken);
 

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)=> await _dbSet.AddRangeAsync(entities, cancellationToken);

 
    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)=>
        predicate == null
            ? await _dbSet.CountAsync(cancellationToken)
            : await _dbSet.CountAsync(predicate, cancellationToken);
    
 
    public void Delete(T entity)=>_dbSet.Remove(entity);
 

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)=>await _dbSet.AnyAsync(predicate, cancellationToken);

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)=>await _dbSet.Where(predicate).ToListAsync(cancellationToken);

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)=>await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
 
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)=>await _dbSet.ToListAsync(cancellationToken);

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)=>await _dbSet.FindAsync(id,cancellationToken);

    public IQueryable<T> GetQueryable()=>_dbSet.AsQueryable();
 
    public void Update(T entity)=> _dbSet.Update(entity);

}