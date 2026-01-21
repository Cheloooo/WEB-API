using System.Data.Common;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WEB_DOMAIN.Interface;

namespace WEB.DAL;

public class UnitOfWork : IUnitofWork, IAsyncDisposable
{
    private readonly AppDbContext.WebApiDbContext _ctx;
    private static readonly AsyncLocal<int> AmbientDept = new();
    public UnitOfWork(AppDbContext.WebApiDbContext ctx) => _ctx = ctx;
    public ValueTask DisposeAsync() => _ctx.DisposeAsync();

    // Write Operations //

    public Task ExecuteAsync(
        Func<CancellationToken, Task> work,
        CancellationToken ct = default)
        => ExecuteCoreAsync<object?>(
            async (_, __, c) =>
            {
                await work(c);
                return null;
            }, ct);

    public Task<TResult> ExecuteAsync<TResult>(
        Func<CancellationToken, Task<TResult>> work,
        CancellationToken ct = default)
        => ExecuteCoreAsync(
            async (_, __, c) => await work(c), ct);

    public Task ExecuteAsync(
        Func<DbConnection, DbTransaction, CancellationToken, Task> work,
        CancellationToken ct = default)
        => ExecuteCoreAsync<object?>(
            async (cnn, tx, c) =>
            {
                await work(cnn, tx, c);
                return null;
            }, ct);

    public Task<TResult> ExecuteAsync<TResult>(
        Func<DbConnection, DbTransaction, CancellationToken, Task<TResult>> work,
        CancellationToken ct = default)
        => ExecuteCoreAsync(work, ct);

    // --------------------
    // READ-ONLY OPERATIONS
    // --------------------

    public Task ExecuteReadOnlyAsync(
        Func<CancellationToken, Task> work,
        CancellationToken ct = default)
        => work(ct);

    public Task<TResult> ExecuteReadOnlyAsync<TResult>(
        Func<CancellationToken, Task<TResult>> work,
        CancellationToken ct = default)
        => work(ct);


    private async Task<TResult> ExecuteCoreAsync<TResult>(
        Func<DbConnection, DbTransaction, CancellationToken, Task<TResult>> work, CancellationToken ct)
    {
        if(!_ctx.Database.IsRelational())
            throw new InvalidOperationException("This UnitOfWork requires a relational EF Core provider.");
        var strategy = _ctx.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            var isOuter = AmbientDept.Value == 0;
            AmbientDept.Value++;
            try
            {
                if (isOuter)
                {
                    await using var tx = await _ctx.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, ct);
                    var conn = _ctx.Database.GetDbConnection();
                    var dbtx = tx.GetDbTransaction();
                    var result = await work(conn, dbtx, ct);
                    await _ctx.SaveChangesAsync(ct);
                    await tx.CommitAsync(ct);
                    return result;
                }
                else
                {
                    var conn = _ctx.Database.GetDbConnection();
                    var current = _ctx.Database.CurrentTransaction
                                  ?? throw new InvalidOperationException("No ambient EF transaction found in nested scope");
                    var dbtx = current.GetDbTransaction();
                    return await work(conn, dbtx, ct);

                }
            }
            catch
            {
                if (_ctx.Database.CurrentTransaction is not null)
                    await _ctx.Database.RollbackTransactionAsync(ct);
                throw;
            }
            finally
            {
                AmbientDept.Value--;
            }
        });
    }
  
}