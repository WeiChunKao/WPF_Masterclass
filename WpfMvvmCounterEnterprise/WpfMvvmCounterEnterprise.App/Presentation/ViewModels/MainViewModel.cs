using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using WpfMvvmCounterEnterprise.App.Application.Services;
using WpfMvvmCounterEnterprise.App.Presentation.Commands;

namespace WpfMvvmCounterEnterprise.App.Presentation.ViewModels;

public sealed class MainViewModel : ViewModelBase
{
    private readonly ILogger<MainViewModel> _logger;
    private int _count;
    private bool _isBusy;
    private string _statusText = "Ready";

    public int Count
    {
        get => _count;
        private set
        {
            if (SetProperty(ref _count, value))
                RaiseAllCanExecuteChanged();
        }
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (SetProperty(ref _isBusy, value))
                RaiseAllCanExecuteChanged();
        }
    }

    public string StatusText
    {
        get => _statusText;
        private set => SetProperty(ref _statusText, value);
    }

    public ICommand IncrementCommand { get; }
    public ICommand DecrementCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand SimulateWorkCommand { get; }

    internal RelayCommand IncrementInternal { get; }
    internal RelayCommand DecrementInternal { get; }
    internal RelayCommand ResetInternal { get; }
    internal AsyncRelayCommand SimulateWorkInternal { get; }

    public MainViewModel(ICounterService counterService, ILogger<MainViewModel> logger)
    {
        var counterService1 = counterService;
        _logger = logger;

        IncrementInternal = new RelayCommand(
            execute: () => Count = counterService1.Increment(Count),
            canExecute: () => !IsBusy);

        DecrementInternal = new RelayCommand(
            execute: () => Count = counterService1.Decrement(Count),
            canExecute: () => !IsBusy);

        ResetInternal = new RelayCommand(
            execute: () => Count = counterService1.Reset(),
            canExecute: () => !IsBusy && Count != 0);

        SimulateWorkInternal = new AsyncRelayCommand(
            executeAsync: SimulateWorkAsync,
            canExecute: () => !IsBusy);

        IncrementCommand = IncrementInternal;
        DecrementCommand = DecrementInternal;
        ResetCommand = ResetInternal;
        SimulateWorkCommand = SimulateWorkInternal;
    }

    private async Task SimulateWorkAsync()
    {
        IsBusy = true;
        StatusText = "Working... (2s)";
        _logger.LogInformation("Simulate work started at {Time}", DateTimeOffset.Now);

        try
        {
            await Task.Delay(2000);
            StatusText = "Done ✅";
            _logger.LogInformation("Simulate work finished at {Time}", DateTimeOffset.Now);
        }
        catch (Exception ex)
        {
            StatusText = "Failed ❌";
            _logger.LogError(ex, "Simulate work failed");
            throw;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void RaiseAllCanExecuteChanged()
    {
        IncrementInternal.RaiseCanExecuteChanged();
        DecrementInternal.RaiseCanExecuteChanged();
        ResetInternal.RaiseCanExecuteChanged();
        SimulateWorkInternal.RaiseCanExecuteChanged();
    }
}
