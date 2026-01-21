using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WEB_DOMAIN.Interface;
using WEB_DOMAIN.Resolver;

namespace WEB.DAL.Repository;

public class Repository<T> : IRepository<T> where T : class, IEntity
{
    protected readonly AppDbContext.WebApiDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext.WebApiDbContext context)
    {
        _context = context ?? throw new ArgumentException(nameof(context));
        _dbSet = _context.Set<T>();
    }
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default, bool asNoTracking = true,
        params string[]? includePaths)
    {
        IQueryable<T> query = _dbSet;
        if (asNoTracking)
            query = query.AsNoTracking();
        
        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (includePaths != null)
        {
            foreach (var include in includePaths)
                query = query.Include(include);
        }

        return await query.ToListAsync(ct);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default, params string[]? includePaths)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();
        if (includePaths?.Length > 0)
        {
            foreach (var include in includePaths)
            {
                query = query.Include(include);
            }
        }

        var keyName = EntityKeyResolver.GetMappedKeyPropertyName<T>();
        return await query
            .Where(e => EF.Property<Guid>(e, keyName) != Guid.Empty && EF.Property<Guid>(e, keyName) == id)
            .FirstOrDefaultAsync();
    }

    public async Task<T?> GetByKeysAsync(CancellationToken ct = default, params object?[] keyValues)
    => await _dbSet.FindAsync(keyValues, ct);

    public async Task AddAsync(T entity, CancellationToken ct = default) => await _dbSet.AddAsync(entity, ct).AsTask();

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default) =>
        _dbSet.AddRangeAsync(entities, ct);

    public void Update(T entity, Expression<Func<T, object?>>[] data)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;

        foreach (var nav in data)
        {
            var memberExpression = nav.Body as MemberExpression ?? (nav.Body is UnaryExpression unary ? unary.Operand as MemberExpression : null);
            if (memberExpression == null) continue;
            var propertyInfor = memberExpression.Member as PropertyInfo;
            if (propertyInfor == null) continue;

            var value = propertyInfor.GetValue(entity);

            if (value != null)
            {
                var propertyType = propertyInfor.PropertyType;
                if (typeof(IEnumerable).IsAssignableFrom(propertyType) && propertyType != typeof(string))
                {
                    var collection = value as IEnumerable;
                    if (collection != null)
                    {
                        foreach (var item in collection)
                        {
                            var itemType = item.GetType();
                            var idProperty = itemType.GetProperty("Id");
                            if (idProperty != null)
                            {
                                var idValue = idProperty.GetValue(item);
                                if (idValue == null || (idValue is Guid guid && guid == Guid.Empty))
                                {
                                    _context.Entry(item).State = EntityState.Added;
                                }
                                else
                                {
                                    _context.Entry(item).State = EntityState.Modified;
                                }
                            }
                            else
                            {
                                _context.Entry(item).State = EntityState.Modified;
                            }
                        }
                    }
                    else
                    {
                        _context.Entry(entity).Reference(nav).TargetEntry.State = EntityState.Modified;
                    }
                }
            }
        }
    }

    public void Delete(T entity) => _dbSet.Remove(entity);

    public async Task DeleteRangeAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        if (ids == null || !ids.Any())
            return;
        var keyName = EntityKeyResolver.GetMappedKeyPropertyName<T>();
        var entities = await _dbSet
            .Where(e => EF.Property<Guid>(e, keyName) != Guid.Empty && ids.Contains(EF.Property<Guid>(e, keyName)))
            .ToListAsync(ct);
        if (entities.Count == 0)
            return;
        _dbSet.RemoveRange(entities);
    }

    

    public IQueryable<T> Query(bool asNoTracking = true)
    {
       return asNoTracking ? _dbSet.AsNoTracking() : _dbSet;
    }

    public void Update(T entity, Expression<Func<T, object?>> data)
    {
        throw new NotImplementedException();
    }
}