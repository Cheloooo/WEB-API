using Microsoft.EntityFrameworkCore;
using WEB_DOMAIN.Entity;
using WEB_DOMAIN.Interface;

namespace WEB.DAL.Repository;

public class BaseEntityRepository<T> : IBaseEntityRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext.WebApiDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseEntityRepository(AppDbContext.WebApiDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    public void softDelete(T entity)
    {
        entity.IsActive = false;
        entity.DeletedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public Task softDeleteListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        if (ids == null || !ids.Any())
            return;
        
        var entities = await _dbSet
            .Where(e => ids.Contains(EF.Property<Guid>(e, "Id")))
            .ToListAsync(ct);
        if (!entities.Any()) return;
        foreach (var entity in entities)
        {
            entity.IsActive = false;
            entity.DeletedAt = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}