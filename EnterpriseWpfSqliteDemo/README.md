# EnterpriseWpfSqliteDemo (Enterprise)

✅ WPF + MVVM + Generic Host + DI + EF Core SQLite + Migrations（企業級分層）

## 專案分層
- EnterpriseWpfSqliteDemo.UI：WPF UI（MVVM / ICommand / ObservableCollection）
- EnterpriseWpfSqliteDemo.Application：Service / Use-cases / Interfaces
- EnterpriseWpfSqliteDemo.Domain：Entities（純領域）
- EnterpriseWpfSqliteDemo.Infrastructure：EF Core / SQLite / Repository / Migrations

## DB 預設位置
`%LocalAppData%\EnterpriseWpfSqliteDemo\app.db`

## Migration（Package Manager Console）
- Default project：EnterpriseWpfSqliteDemo.Infrastructure
- Startup project：EnterpriseWpfSqliteDemo.UI

```powershell
Add-Migration AddNewField
Update-Database
```

## 執行
開啟 EnterpriseWpfSqliteDemo.sln → Startup Project 設 EnterpriseWpfSqliteDemo.UI → F5
