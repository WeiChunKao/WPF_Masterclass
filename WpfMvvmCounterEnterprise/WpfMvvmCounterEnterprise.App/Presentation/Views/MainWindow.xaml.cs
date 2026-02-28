using System.Windows;
using WpfMvvmCounterEnterprise.App.Presentation.ViewModels;

namespace WpfMvvmCounterEnterprise.App.Presentation.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
