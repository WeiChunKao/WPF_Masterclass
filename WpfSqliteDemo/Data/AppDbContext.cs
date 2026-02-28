using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;
using WpfSqliteDemo.Models;

namespace WpfSqliteDemo.Data;

public class AppDbContext : DbContext
{
    public DbSet<TodoItem> Todos => Set<TodoItem>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>(e =>
        {
            e.Property(x => x.Title).HasMaxLength(200).IsRequired();
            e.HasIndex(x => x.CreatedAt);
        });
    }
}
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dbDir = Path.Combine(folder, "WpfSqliteDemo");
        Directory.CreateDirectory(dbDir);

        var dbPath = Path.Combine(dbDir, "app.db");
        var cs = $"Data Source={dbPath}";

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(cs)
            .Options;

        return new AppDbContext(options);
    }
}