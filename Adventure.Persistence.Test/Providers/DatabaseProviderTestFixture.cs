using Adventure.Persistence.Providers;
using Microsoft.Azure.Documents;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Adventure.Persistence.Test.Providers
{
    public class DatabaseProviderTestFixture
    {
        private readonly string _databaseId;
        private readonly IDocumentClient _documentClient;
        private readonly IDatabaseProvider _databaseProvider;

        public DatabaseProviderTestFixture(IDocumentClientFactory documentClientFactory)
        {
            _documentClient = documentClientFactory.GetClient();
            _databaseId = documentClientFactory.GetDatabaseId();
            _databaseProvider = new DatabaseProvider(null);
        }

        public async Task RunOrderedTest()
        {
            await CreateOrGetDb();

            await GetDbSelfLink();
        }

        private async Task GetDbSelfLink()
        {
            var dbSelfLink = await _databaseProvider.GetDbSelfLink();

            Assert.NotNull(dbSelfLink);

            Assert.NotNull(
                this._documentClient.CreateDatabaseQuery()
                    .Where(d => d.SelfLink == dbSelfLink)
                    .AsEnumerable()
                    .FirstOrDefault());
        }

        private async Task CreateOrGetDb()
        {
            await CreateDb();

            await GetDb();
        }

        private async Task CreateDb()
        {
            var database = _documentClient.CreateDatabaseQuery()
                .Where(d => d.Id == _databaseId)
                .AsEnumerable()
                .FirstOrDefault();

            Assert.NotNull(
                await _databaseProvider.CreateOrGetDb());

            Assert.NotNull(
                this._documentClient.CreateDatabaseQuery()
                    .Where(d => d.Id == _databaseId)
                    .AsEnumerable()
                    .FirstOrDefault());
        }

        private async Task GetDb()
        {
            Assert.NotNull(
                _documentClient.CreateDatabaseQuery()
                    .Where(d => d.Id == _databaseId)
                    .AsEnumerable()
                    .FirstOrDefault());

            Assert.NotNull(
                await _databaseProvider.CreateOrGetDb());
        }
    }
}
