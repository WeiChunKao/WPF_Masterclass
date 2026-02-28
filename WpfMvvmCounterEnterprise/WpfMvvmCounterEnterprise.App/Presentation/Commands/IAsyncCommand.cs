using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfMvvmCounterEnterprise.App.Presentation.Commands;

public interface IAsyncCommand : ICommand
{
    Task ExecuteAsync(object? parameter = null);
}
