# WpfMvvmCounterEnterprise

這是一個「給初學者但符合企業常見做法」的 WPF MVVM 範例專案，包含：

- MVVM：ViewModelBase + SetProperty + PropertyChanged
- ICommand：RelayCommand / RelayCommand<T>
- Async ICommand：AsyncRelayCommand + IAsyncCommand（可 await）
- IsBusy 驅動 CanExecute：按鈕會自動 Disabled
- HostBuilder + DI：MainWindow / MainViewModel / Services 全部用 DI
- NUnit：測 ViewModel、PropertyChanged、AsyncCommand

## 開啟與執行

用 Visual Studio 2022 開啟 `WpfMvvmCounterEnterprise.sln`  
設定啟動專案為 `WpfMvvmCounterEnterprise.App` 後 F5。

## CLI

```bash
dotnet restore
dotnet build
dotnet test
```
