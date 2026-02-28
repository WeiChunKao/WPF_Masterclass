using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnterpriseWpfSqliteDemo.UI.ViewModels.Commands;

/// <summary>
/// async ICommand：避免 UI thread 被阻塞，並避免連點併發
/// </summary>
public interface IAsyncCommand : ICommand
{
    Task ExecuteAsync(object? parameter);
    void RaiseCanExecuteChanged();
}

public sealed class AsyncRelayCommand : IAsyncCommand
{
    private readonly Func<Task> _execute;
    private readonly Func<bool>? _canExecute;
    private bool _isExecuting;

    public AsyncRelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
        => !_isExecuting && (_canExecute?.Invoke() ?? true);

    public async void Execute(object? parameter)
        => await ExecuteAsync(parameter);

    public async Task ExecuteAsync(object? parameter)
    {
        if (!CanExecute(parameter)) return;
        try
        {
            _isExecuting = true;
            RaiseCanExecuteChanged();
            await _execute();
        }
        finally
        {
            _isExecuting = false;
            RaiseCanExecuteChanged();
        }
    }

    public void RaiseCanExecuteChanged()
        => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

public sealed class AsyncRelayCommand<T> : IAsyncCommand
{
    private readonly Func<T?, Task> _execute;
    private readonly Func<T?, bool>? _canExecute;
    private bool _isExecuting;

    public AsyncRelayCommand(Func<T?, Task> execute, Func<T?, bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        if (_isExecuting) return false;
        var p = parameter is T t ? t : default;
        return _canExecute?.Invoke(p) ?? true;
    }

    public async void Execute(object? parameter)
        => await ExecuteAsync(parameter);

    public async Task ExecuteAsync(object? parameter)
    {
        if (!CanExecute(parameter)) return;
        try
        {
            _isExecuting = true;
            RaiseCanExecuteChanged();
            var p = parameter is T t ? t : default;
            await _execute(p);
        }
        finally
        {
            _isExecuting = false;
            RaiseCanExecuteChanged();
        }
    }

    public void RaiseCanExecuteChanged()
        => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
