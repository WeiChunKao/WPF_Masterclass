using EnterpriseWpfSqliteDemo.Domain.Entities;

namespace EnterpriseWpfSqliteDemo.Application.Abstractions;

/// <summary>
/// Service 抽象（可放業務規則、驗證、交易邏輯）
/// </summary>
public interface ITodoService
{
    Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken ct = default);
    Task<int> AddAsync(string title, CancellationToken ct = default);
    Task UpdateAsync(TodoItem item, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
