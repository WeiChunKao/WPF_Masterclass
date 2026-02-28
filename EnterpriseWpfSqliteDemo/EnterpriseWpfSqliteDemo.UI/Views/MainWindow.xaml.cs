using System.Windows;
using EnterpriseWpfSqliteDemo.UI.ViewModels;

namespace EnterpriseWpfSqliteDemo.UI.Views;

/// <summary>
/// View：只負責 UI 與 Binding
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
