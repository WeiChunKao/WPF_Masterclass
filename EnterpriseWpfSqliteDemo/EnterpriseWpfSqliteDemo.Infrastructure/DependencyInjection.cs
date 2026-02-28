using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using EnterpriseWpfSqliteDemo.Application.Abstractions;
using EnterpriseWpfSqliteDemo.Infrastructure.Persistence;
using EnterpriseWpfSqliteDemo.Infrastructure.Repositories;

namespace EnterpriseWpfSqliteDemo.Infrastructure;

/// <summary>
/// Infrastructure DI 註冊
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // 優先讀 appsettings.json 的 ConnectionStrings:Sqlite
        var cs = config.GetConnectionString("Sqlite") ?? BuildDefaultConnectionString();

        services.AddDbContextFactory<AppDbContext>(options =>
        {
            options.UseSqlite(cs);
        });

        services.AddSingleton<ITodoRepository, TodoRepository>();
        return services;
    }

    private static string BuildDefaultConnectionString()
    {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dbDir = Path.Combine(folder, "EnterpriseWpfSqliteDemo");
        Directory.CreateDirectory(dbDir);
        var dbPath = Path.Combine(dbDir, "app.db");
        return $"Data Source={dbPath}";
    }
}
