using System.Data.Common;
using LanguageExt.Common;

namespace WEB_DOMAIN.Interface;

public interface IDapperUnitOfWork : IAsyncDisposable
{
    Task ExecuteAsync(Func<DbConnection, DbTransaction, CancellationToken , Task> work, CancellationToken ct = default);
    Task<Result<>> ExecuteAsync<TResult>(Func<DbConnection, DbTransaction, CancellationToken, Task<TResult>> work, CancellationToken ct = default);
    
}