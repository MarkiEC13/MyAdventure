using Adventure.Persistence.Providers;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Adventure.Persistence.Test.Providers
{
    public class GenericCollectionProviderTestFixture
    {
        private readonly string _databaseId;
        private readonly IDocumentClient _documentClient;
        private readonly ICollectionProvider _collectionProvider;

        public GenericCollectionProviderTestFixture(IDocumentClientFactory documentClientFactory)
        {            
            _documentClient = documentClientFactory.GetClient();
            _databaseId = documentClientFactory.GetDatabaseId();

            _collectionProvider = new GenericCollectionProvider<TestDocument>(
                documentClientFactory,
                new DatabaseProvider(documentClientFactory));
        }

        public async Task RunOrderedTest()
        {
            await CreateOrGetCollection();

            await GetDocumentsLink();
        }

        private async Task GetDocumentsLink()
        {
            var docLink = await _collectionProvider.GetCollectionDocumentsLink();

            Assert.NotNull(docLink);

            Assert.NotNull(
                _documentClient.CreateDocumentCollectionQuery(
                        UriFactory.CreateDatabaseUri(_databaseId))
                    .ToList()
                    .FirstOrDefault(c => c.DocumentsLink == docLink));
        }

        private async Task CreateOrGetCollection()
        {
            await CreateCollection();

            await GetCollection();
        }

        private async Task CreateCollection()
        {
            var collection = _documentClient.CreateDocumentCollectionQuery(
                    UriFactory.CreateDatabaseUri(_databaseId))
                .Where(c => c.Id == typeof(TestDocument).Name)
                .AsEnumerable()
                .FirstOrDefault();

            Assert.NotNull(
                await _collectionProvider.CreateOrGetCollection());

            Assert.NotNull(
                _documentClient.CreateDocumentCollectionQuery(
                        UriFactory.CreateDatabaseUri(_databaseId))
                    .Where(c => c.Id == typeof(TestDocument).Name)
                    .AsEnumerable()
                    .FirstOrDefault());
        }

        private async Task GetCollection()
        {
            Assert.NotNull(
                _documentClient.CreateDocumentCollectionQuery(
                        UriFactory.CreateDatabaseUri(_databaseId))
                    .Where(c => c.Id == typeof(TestDocument).Name)
                    .AsEnumerable()
                    .FirstOrDefault());

            Assert.NotNull(
                await _collectionProvider.CreateOrGetCollection());
        }
    }
}
