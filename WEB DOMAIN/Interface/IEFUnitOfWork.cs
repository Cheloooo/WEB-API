namespace WEB_DOMAIN.Interface;

public interface IEFUnitOfWork : IAsyncDisposable
{
    Task ExecuteAsync(Func<CancellationToken, Task> work, CancellationToken ct = default);
    Task<TResult> ExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> work, CancellationToken ct = default);
    Task ExecuteReadOnlyAsync(Func<CancellationToken, Task> work, CancellationToken ct = default);
    Task<TResult> ExecuteReadOnlyAsync<TResult>(Func<CancellationToken, Task <TResult>> work, CancellationToken ct = default);
    public async ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }
}