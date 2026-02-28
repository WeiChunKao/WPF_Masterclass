using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Windows;
using WpfSqliteDemo.Data;

namespace WpfSqliteDemo;

public partial class App : Application
{
    // WPF 沒有內建 DI 的最小可行做法：把 Options 存起來，需要時 new DbContext(options)
    public static DbContextOptions<AppDbContext> DbOptions { get; private set; } = default!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 建議放到 LocalAppData，避免需要管理員權限
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dbDir = Path.Combine(folder, "WpfSqliteDemo");
        Directory.CreateDirectory(dbDir);

        var dbPath = Path.Combine(dbDir, "app.db");
        var connectionString = $"Data Source={dbPath}";

        DbOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connectionString)
            //.EnableSensitiveDataLogging() // Debug 時可開，Release 建議關
            .Options;

        using (var db = new AppDbContext(DbOptions))
        {
            // 沒有就建立、有就升級（Migration）
            db.Database.MigrateAsync();
        }

        new MainWindow().Show();
    }
}
