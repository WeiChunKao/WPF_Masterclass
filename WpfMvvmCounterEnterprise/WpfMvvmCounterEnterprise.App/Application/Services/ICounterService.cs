namespace WpfMvvmCounterEnterprise.App.Application.Services;

public interface ICounterService
{
    int Increment(int current);
    int Decrement(int current);
    int Reset();
}
