using Microsoft.Extensions.DependencyInjection;

namespace Adventure.Core.DiHelpers
{
    public static class CatelogExtention
    {
        public static void RegisterModule<T>(this IServiceCollection service) where T : ICatalog, new()
        {
            var module = new T();
            module.RegisterServices(service);
        }
    }
}
