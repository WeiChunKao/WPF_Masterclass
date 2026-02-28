# WpfSqliteDemo (WPF + SQLite + EF Core)

## 需求
- Visual Studio 2022 或以上
- .NET 8 SDK

## 開啟方式
1. 解壓縮後用 Visual Studio 開啟 `WpfSqliteDemo.csproj`
2. 先還原 NuGet
3. 執行專案（F5）

資料庫會建立在：
`%LocalAppData%\WpfSqliteDemo\app.db`

## Migration（可選）
若你想自己建立 Migration：

```powershell
Add-Migration InitialCreate
Update-Database
```

> 注意：這會在專案目錄下產生 Migrations 資料夾。
