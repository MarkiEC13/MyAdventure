using Adventure.Core.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using System;

namespace Adventure.Persistence
{
    public class DocumentClientFactory : IDocumentClientFactory
    {
        private readonly string _databaseId;
        private readonly Lazy<IDocumentClient> _documentClient;

        public DocumentClientFactory(IOptions<DocumentDbSettings> documentDbSettings)
        {
            if (documentDbSettings == null) throw new ArgumentNullException(nameof(documentDbSettings));

            _databaseId = documentDbSettings.Value.DatabaseId;
            _documentClient = new Lazy<IDocumentClient>(() =>
                new DocumentClient(new Uri(documentDbSettings.Value.Uri), documentDbSettings.Value.AuthKey, 
                new ConnectionPolicy
                {
                    RetryOptions =
                    {
                        MaxRetryAttemptsOnThrottledRequests = documentDbSettings.Value.MaxRetryAttemptsOnThrottledRequests,
                        MaxRetryWaitTimeInSeconds = documentDbSettings.Value.MaxRetryWaitTimeInSeconds
                    }
                }));
        }

        public IDocumentClient GetClient()
        {
            return _documentClient.Value;
        }

        public string GetDatabaseId()
        {
            return _databaseId;
        }
    }
}
