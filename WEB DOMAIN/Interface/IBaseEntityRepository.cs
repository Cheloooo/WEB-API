using WEB_DOMAIN.Entity;

namespace WEB_DOMAIN.Interface;

public interface IBaseEntityRepository<T> where T : BaseEntity
{
    void softDelete(T entity);
    Task softDeleteListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

}