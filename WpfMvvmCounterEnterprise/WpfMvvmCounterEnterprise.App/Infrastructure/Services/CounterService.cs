using WpfMvvmCounterEnterprise.App.Application.Services;

namespace WpfMvvmCounterEnterprise.App.Infrastructure.Services;

public sealed class CounterService : ICounterService
{
    public int Increment(int current) => current + 1;
    public int Decrement(int current) => current - 1;
    public int Reset() => 0;
}
