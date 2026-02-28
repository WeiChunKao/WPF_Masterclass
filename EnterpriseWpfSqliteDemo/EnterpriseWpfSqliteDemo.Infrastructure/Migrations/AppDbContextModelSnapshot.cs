using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using EnterpriseWpfSqliteDemo.Infrastructure.Persistence;

#nullable disable

namespace EnterpriseWpfSqliteDemo.Infrastructure.Migrations;

[DbContext(typeof(AppDbContext))]
partial class AppDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

        modelBuilder.Entity("EnterpriseWpfSqliteDemo.Domain.Entities.TodoItem", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("INTEGER");

            b.Property<DateTime>("CreatedAtUtc")
                .HasColumnType("TEXT");

            b.Property<bool>("IsDone")
                .HasColumnType("INTEGER");

            b.Property<string>("Title")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.HasIndex("CreatedAtUtc");

            b.ToTable("Todos");
        });
    }
}
