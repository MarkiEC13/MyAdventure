using Microsoft.Azure.Documents;

namespace Adventure.Persistence
{
    public interface IDocumentClientFactory
    {
        IDocumentClient GetClient();

        string GetDatabaseId();
    }
}
