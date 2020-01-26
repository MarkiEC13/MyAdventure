using Microsoft.Azure.Documents;
using System.Threading.Tasks;

namespace Adventure.Persistence.Providers
{
    public interface ICollectionProvider
    {
        Task<DocumentCollection> CreateOrGetCollection();

        Task<string> GetCollectionDocumentsLink();
    }
}
