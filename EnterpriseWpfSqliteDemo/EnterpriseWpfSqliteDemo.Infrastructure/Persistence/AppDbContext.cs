using Microsoft.EntityFrameworkCore;
using EnterpriseWpfSqliteDemo.Domain.Entities;

namespace EnterpriseWpfSqliteDemo.Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext（Infrastructure）
/// - Domain / Application 不依賴 EF
/// </summary>
public sealed class AppDbContext : DbContext
{
    public DbSet<TodoItem> Todos => Set<TodoItem>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.ToTable("Todos");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                  .HasMaxLength(200)
                  .IsRequired();

            entity.Property(x => x.CreatedAtUtc)
                  .IsRequired();

            entity.HasIndex(x => x.CreatedAtUtc);
        });
    }
}
