using System.Linq.Expressions;

namespace ProjectGym.Domain.Interfaces;

public interface IRepository<T> where T: class
{
    Task <T?> GetByIdAsync(int id, CancellationToken cancellationToken=default);

    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken=default);

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>>
    predicate, CancellationToken cancellationToken);

    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>
    predicate, CancellationToken cancellationToken);

    Task AddAsync(T entity, CancellationToken cancellationToken=default);

    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);

    void Update(T entity);

    void Delete(T entity);

    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken=default);

    Task <int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken=default);

    IQueryable<T> GetQueryable();

    }