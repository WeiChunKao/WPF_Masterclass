using EnterpriseWpfSqliteDemo.Domain.Entities;

namespace EnterpriseWpfSqliteDemo.Application.Abstractions;

/// <summary>
/// Repository 抽象（Application 層只定義介面，不碰資料庫）
/// </summary>
public interface ITodoRepository
{
    Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken ct = default);
    Task<TodoItem?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> AddAsync(TodoItem item, CancellationToken ct = default);
    Task UpdateAsync(TodoItem item, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
