using System.Linq.Expressions;

namespace WEB_DOMAIN.Interface;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default, bool asNoTracking = true, params string[]? ignoreProperties);
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default, params string[]? ignoreProperties);
    Task<T?> GetByKeysAsync(CancellationToken ct = default, params object?[] keyValues);
    Task AddAsync(T entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
    void Update(T entity, Expression<Func<T, object?>> data);
    void Delete(T entity);
    Task DeleteRangeAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    IQueryable<T> Query(bool asNoTracking = true);
    
    
    
}