using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using EnterpriseWpfSqliteDemo.Application.Abstractions;
using EnterpriseWpfSqliteDemo.Domain.Entities;
using EnterpriseWpfSqliteDemo.UI.ViewModels.Commands;

namespace EnterpriseWpfSqliteDemo.UI.ViewModels;

/// <summary>
/// ViewModel：透過 ITodoService 操作資料
/// </summary>
public sealed class MainViewModel : ObservableObject
{
    private readonly ITodoService _service;
    private readonly ILogger<MainViewModel> _logger;
    private readonly SemaphoreSlim _gate = new(1, 1);

    public ObservableCollection<TodoItem> Todos { get; } = new();

    private TodoItem? _selectedTodo;
    public TodoItem? SelectedTodo
    {
        get => _selectedTodo;
        set => SetProperty(ref _selectedTodo, value);
    }

    private string _newTitle = string.Empty;
    public string NewTitle
    {
        get => _newTitle;
        set
        {
            if (SetProperty(ref _newTitle, value))
                AddCommand.RaiseCanExecuteChanged();
        }
    }

    private string _status = "Ready";
    public string Status
    {
        get => _status;
        private set => SetProperty(ref _status, value);
    }

    public IAsyncCommand RefreshCommand { get; }
    public IAsyncCommand AddCommand { get; }
    public IAsyncCommand SaveCommand { get; }
    public IAsyncCommand DeleteCommand { get; }

    public MainViewModel(ITodoService service, ILogger<MainViewModel> logger)
    {
        _service = service;
        _logger = logger;

        RefreshCommand = new AsyncRelayCommand(RefreshAsync);
        AddCommand = new AsyncRelayCommand(AddAsync, () => !string.IsNullOrWhiteSpace(NewTitle));
        SaveCommand = new AsyncRelayCommand<TodoItem>(SaveAsync, todo => todo is not null);
        DeleteCommand = new AsyncRelayCommand<TodoItem>(DeleteAsync, todo => todo is not null);

        _ = RefreshAsync();
    }

    private async Task RefreshAsync()
    {
        await RunExclusive(async ct =>
        {
            Status = "Loading...";
            var items = await _service.GetAllAsync(ct);

            Todos.Clear();
            foreach (var item in items)
                Todos.Add(item);

            Status = $"Loaded: {Todos.Count}";
        });
    }

    private async Task AddAsync()
    {
        var title = (NewTitle ?? string.Empty).Trim();

        await RunExclusive(async ct =>
        {
            await _service.AddAsync(title, ct);
            NewTitle = string.Empty;
            Status = "Added.";
            await RefreshAsync();
        });
    }

    private async Task SaveAsync(TodoItem? todo)
    {
        if (todo is null) return;

        await RunExclusive(async ct =>
        {
            await _service.UpdateAsync(todo, ct);
            Status = $"Saved: #{todo.Id}";
        });
    }

    private async Task DeleteAsync(TodoItem? todo)
    {
        if (todo is null) return;

        await RunExclusive(async ct =>
        {
            await _service.DeleteAsync(todo.Id, ct);
            Status = $"Deleted: #{todo.Id}";
            await RefreshAsync();
        });
    }

    private async Task RunExclusive(Func<CancellationToken, Task> action)
    {
        await _gate.WaitAsync();
        try
        {
            await action(CancellationToken.None);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Operation failed");
            Status = ex.Message;
        }
        finally
        {
            _gate.Release();
        }
    }
}
