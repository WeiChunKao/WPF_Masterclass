using Microsoft.EntityFrameworkCore;
using EnterpriseWpfSqliteDemo.Application.Abstractions;
using EnterpriseWpfSqliteDemo.Domain.Entities;
using EnterpriseWpfSqliteDemo.Infrastructure.Persistence;

namespace EnterpriseWpfSqliteDemo.Infrastructure.Repositories;

/// <summary>
/// Repository 實作（Infrastructure）
/// - 使用 IDbContextFactory：每次操作都建立新的 DbContext
/// </summary>
public sealed class TodoRepository : ITodoRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public TodoRepository(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);
        return await db.Todos
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .ToListAsync(ct);
    }

    public async Task<TodoItem?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);
        return await db.Todos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<int> AddAsync(TodoItem item, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);
        db.Todos.Add(item);
        await db.SaveChangesAsync(ct);
        return item.Id;
    }

    public async Task UpdateAsync(TodoItem item, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);
        db.Todos.Update(item);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);
        db.Todos.Remove(new TodoItem { Id = id });
        await db.SaveChangesAsync(ct);
    }
}
