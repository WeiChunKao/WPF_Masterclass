using System;

namespace EnterpriseWpfSqliteDemo.Domain.Entities;

/// <summary>
/// Domain Entity（領域實體）
/// - 只放「業務語意」的資料與規則
/// - 不依賴 EF Core / WPF / Infrastructure
/// </summary>
public class TodoItem
{
    /// <summary>資料庫主鍵</summary>
    public int Id { get; set; }

    /// <summary>待辦事項標題</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>是否完成</summary>
    public bool IsDone { get; set; }

    /// <summary>建立時間（UTC）</summary>
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
