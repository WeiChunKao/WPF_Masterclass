using Microsoft.Extensions.Logging;
using EnterpriseWpfSqliteDemo.Application.Abstractions;
using EnterpriseWpfSqliteDemo.Domain.Entities;

namespace EnterpriseWpfSqliteDemo.Application.Services;

/// <summary>
/// Application Service
/// - 聚合「業務規則」與資料存取
/// - 這裡示範：新增時做基本驗證、統一紀錄 log
/// </summary>
public sealed class TodoService : ITodoService
{
    private readonly ITodoRepository _repo;
    private readonly ILogger<TodoService> _logger;

    public TodoService(ITodoRepository repo, ILogger<TodoService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public Task<IReadOnlyList<TodoItem>> GetAllAsync(CancellationToken ct = default)
        => _repo.GetAllAsync(ct);

    public async Task<int> AddAsync(string title, CancellationToken ct = default)
    {
        title = (title ?? string.Empty).Trim();

        // 這裡就是「業務規則」：title 必填且長度限制
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));
        if (title.Length > 200)
            throw new ArgumentException("Title length must be <= 200.", nameof(title));

        var item = new TodoItem
        {
            Title = title,
            IsDone = false,
            CreatedAtUtc = DateTime.UtcNow
        };

        var id = await _repo.AddAsync(item, ct);
        _logger.LogInformation("Todo created. Id={Id}, Title={Title}", id, title);
        return id;
    }

    public async Task UpdateAsync(TodoItem item, CancellationToken ct = default)
    {
        if (item.Id <= 0) throw new ArgumentException("Invalid Id.", nameof(item));

        item.Title = (item.Title ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(item.Title))
            throw new ArgumentException("Title is required.", nameof(item.Title));
        if (item.Title.Length > 200)
            throw new ArgumentException("Title length must be <= 200.", nameof(item.Title));

        await _repo.UpdateAsync(item, ct);
        _logger.LogInformation("Todo updated. Id={Id}", item.Id);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        if (id <= 0) return;
        await _repo.DeleteAsync(id, ct);
        _logger.LogInformation("Todo deleted. Id={Id}", id);
    }
}
