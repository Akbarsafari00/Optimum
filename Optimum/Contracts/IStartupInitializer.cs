namespace Optimum.Contracts;

public interface IStartupInitializer : IInitializer
{
    void AddInitializer(IInitializer? initializer);
}