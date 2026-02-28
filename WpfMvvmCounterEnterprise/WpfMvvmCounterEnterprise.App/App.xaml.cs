using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WpfMvvmCounterEnterprise.App.Application.Services;
using WpfMvvmCounterEnterprise.App.Infrastructure.Services;
using WpfMvvmCounterEnterprise.App.Presentation.ViewModels;
using WpfMvvmCounterEnterprise.App.Presentation.Views;

namespace WpfMvvmCounterEnterprise.App;

public partial class App : System.Windows.Application
{
    private IHost? _host;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _host = Host.CreateDefaultBuilder()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .ConfigureServices(services =>
            {
                // Application -> Infrastructure
                services.AddSingleton<ICounterService, CounterService>();

                // ViewModels
                services.AddTransient<MainViewModel>();

                // Views
                services.AddTransient<MainWindow>();
            })
            .Build();

        _host.Start();

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
}
