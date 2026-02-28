using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Windows;
using EnterpriseWpfSqliteDemo.Application;
using EnterpriseWpfSqliteDemo.Infrastructure;
using EnterpriseWpfSqliteDemo.Infrastructure.Persistence;
using EnterpriseWpfSqliteDemo.UI.Views;

namespace EnterpriseWpfSqliteDemo.UI;

/// <summary>
/// WPF App（企業級寫法）
/// - Generic Host：統一 DI / Config / Logging
/// - Startup 時自動 Migrate DB
/// </summary>
public partial class App : System.Windows.Application
{
    private IHost? _host;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(cfg =>
            {
                cfg.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((ctx, services) =>
            {
                services.AddApplication();
                services.AddInfrastructure(ctx.Configuration);

                services.AddSingleton<ViewModels.MainViewModel>();
                services.AddSingleton<MainWindow>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .Build();

        await _host.StartAsync();

        await MigrateDatabaseAsync(_host.Services);

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host is not null)
        {
            await _host.StopAsync(TimeSpan.FromSeconds(3));
            _host.Dispose();
        }
        base.OnExit(e);
    }

    private static async Task MigrateDatabaseAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();

        await using var db = await dbFactory.CreateDbContextAsync();
        await db.Database.MigrateAsync();
    }
}
