using Microsoft.Extensions.DependencyInjection;
using EnterpriseWpfSqliteDemo.Application.Abstractions;
using EnterpriseWpfSqliteDemo.Application.Services;

namespace EnterpriseWpfSqliteDemo.Application;

/// <summary>
/// Application 層 DI 註冊
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Service 不持有 DbContext，因此可用 Singleton
        services.AddSingleton<ITodoService, TodoService>();
        return services;
    }
}
