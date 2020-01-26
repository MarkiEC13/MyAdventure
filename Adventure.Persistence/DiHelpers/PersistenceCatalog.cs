using Adventure.Core.DiHelpers;
using Adventure.Persistence.Containers;
using Adventure.Persistence.Providers;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.DependencyInjection;

namespace Adventure.Persistence.DiHelpers
{
    public class PersistenceCatalog : ICatalog
    {
        public void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ICollectionProvider, GenericCollectionProvider<Resource>>();
            serviceCollection.AddScoped<IDatabaseProvider, DatabaseProvider>();
            serviceCollection.AddScoped<IDocumentClientFactory, DocumentClientFactory>();
            serviceCollection.AddScoped<IGenericRepository<Doughnut>, GenericRepository<Doughnut>>();
        }
    }
}
