using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;

namespace EnterpriseWpfSqliteDemo.Infrastructure.Persistence;

/// <summary>
/// 給 EF Core Tools 在設計階段建立 DbContext 用（WPF 必備）
/// </summary>
public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dbDir = Path.Combine(folder, "EnterpriseWpfSqliteDemo");
        Directory.CreateDirectory(dbDir);

        var dbPath = Path.Combine(dbDir, "app.db");
        var cs = $"Data Source={dbPath}";

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(cs)
            .Options;

        return new AppDbContext(options);
    }
}
