using Optimum.Contracts;

namespace Optimum;

public class OptimumInitializer : IStartupInitializer 

{
    private readonly IList<IInitializer?> _initializers = (IList<IInitializer?>) new List<IInitializer>();

    public void AddInitializer(IInitializer? initializer)
    {
        if (initializer == null || _initializers.Contains(initializer))
            return;
        _initializers.Add(initializer);
    }

    public async Task InitializeAsync()
    {
        foreach (var initializer in (IEnumerable<IInitializer>) this._initializers)
            await initializer.InitializeAsync();
    }
    
    
   
}