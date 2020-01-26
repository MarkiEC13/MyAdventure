using Microsoft.Extensions.DependencyInjection;

namespace Adventure.Core.DiHelpers
{
    public interface ICatalog
    {
        void RegisterServices(IServiceCollection serviceCollection);
    }
}
