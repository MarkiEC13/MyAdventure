using Adventure.Core.Models;
using Adventure.Persistence.Test.Providers;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Configuration;
using Xunit;

namespace Adventure.Persistence.Test
{
    public class IntegrationTestFixture
    {
        [Fact(Skip = "Integration Test")]
        public async void MainTest()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var dbSettings = Options.Create(new DocumentDbSettings
            {
                AuthKey = config["AuthKey"],
                DatabaseId = config["DatabaseId"],
                Uri = config["Uri"],
                MaxRetryAttemptsOnThrottledRequests = int.Parse(config["MaxRetryAttemptsOnThrottledRequests"]),
                MaxRetryWaitTimeInSeconds = int.Parse(config["MaxRetryWaitTimeInSeconds"])
            });
            var documentClientFactory = new DocumentClientFactory(dbSettings);
            var documentClient = documentClientFactory.GetClient();
            try
            {
                
                await new DatabaseProviderTestFixture(documentClientFactory).RunOrderedTest();

                await new GenericCollectionProviderTestFixture(documentClientFactory).RunOrderedTest();

                await new GenericRepositoryTestFixture(documentClientFactory).RunOrderedTest();
            }
            finally
            {
                documentClient.DeleteDatabaseAsync(
                    UriFactory.CreateDatabaseUri(documentClientFactory.GetDatabaseId()))
                    .Wait();
            }
        }
    }
}
