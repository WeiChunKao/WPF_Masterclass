using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using WpfMvvmCounterEnterprise.App.Application.Services;
using WpfMvvmCounterEnterprise.App.Infrastructure.Services;
using WpfMvvmCounterEnterprise.App.Presentation.Commands;
using WpfMvvmCounterEnterprise.App.Presentation.ViewModels;

namespace WpfMvvmCounterEnterprise.Tests;

[TestFixture]
public class MainViewModelTests
{
    private static MainViewModel CreateVm(ICounterService? svc = null)
    {
        svc ??= new CounterService();
        var logger = NullLogger<MainViewModel>.Instance;
        return new MainViewModel(svc, logger);
    }

    [Test]
    public void Increment_ShouldIncreaseCount()
    {
        var vm = CreateVm();
        vm.IncrementCommand.Execute(null);
        Assert.That(vm.Count, Is.EqualTo(1));
    }

    [Test]
    public void Decrement_ShouldDecreaseCount()
    {
        var vm = CreateVm();
        vm.DecrementCommand.Execute(null);
        Assert.That(vm.Count, Is.EqualTo(-1));
    }

    [Test]
    public void Reset_CanExecute_ShouldBeFalse_WhenCountIsZero()
    {
        var vm = CreateVm();
        Assert.That(vm.ResetCommand.CanExecute(null), Is.False);
    }

    [Test]
    public void Reset_CanExecute_ShouldBeTrue_WhenCountIsNotZero()
    {
        var vm = CreateVm();
        vm.IncrementCommand.Execute(null);
        Assert.That(vm.ResetCommand.CanExecute(null), Is.True);
    }

    [Test]
    public void Reset_ShouldSetCountToZero()
    {
        var vm = CreateVm();
        vm.IncrementCommand.Execute(null);
        vm.IncrementCommand.Execute(null);

        vm.ResetCommand.Execute(null);

        Assert.That(vm.Count, Is.EqualTo(0));
        Assert.That(vm.ResetCommand.CanExecute(null), Is.False);
    }

    [Test]
    public void PropertyChanged_ShouldFire_ForCount()
    {
        var vm = CreateVm();
        var fired = new List<string>();

        vm.PropertyChanged += (_, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.PropertyName))
                fired.Add(e.PropertyName!);
        };

        vm.IncrementCommand.Execute(null);

        Assert.That(fired, Does.Contain(nameof(MainViewModel.Count)));
    }

    [Test]
    public void IsBusy_ShouldDisableCommands_DuringAsyncWork()
    {
        var vm = CreateVm();

        Assert.That(vm.IncrementCommand.CanExecute(null), Is.True);
        Assert.That(vm.SimulateWorkCommand.CanExecute(null), Is.True);

        var asyncCmd = (IAsyncCommand)vm.SimulateWorkCommand;

        // 開始後立刻忙碌
        var running = asyncCmd.ExecuteAsync();
        Assert.That(vm.IsBusy, Is.True);
        Assert.That(vm.IncrementCommand.CanExecute(null), Is.False);
        Assert.That(vm.DecrementCommand.CanExecute(null), Is.False);

        // 等完成（2 秒）
        Assert.That(async () => await running, Throws.Nothing);

        Assert.That(vm.IsBusy, Is.False);
        Assert.That(vm.IncrementCommand.CanExecute(null), Is.True);
    }

    [Test]
    public async Task SimulateWorkCommand_ShouldUpdateStatusText()
    {
        var vm = CreateVm();
        var asyncCmd = (IAsyncCommand)vm.SimulateWorkCommand;

        await asyncCmd.ExecuteAsync();

        Assert.That(vm.StatusText, Is.EqualTo("Done ✅"));
    }
}
