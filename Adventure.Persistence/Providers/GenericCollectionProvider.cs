using Microsoft.Azure.Documents;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure.Persistence.Providers
{
    public class GenericCollectionProvider<TDocument> : ICollectionProvider
    {
        private readonly IDocumentClient _documentClient;
        private readonly IDatabaseProvider _databaseProvider;

        public GenericCollectionProvider(
            IDocumentClientFactory documentClientFactory,
            IDatabaseProvider databaseProvider)
        {
            if (documentClientFactory == null) throw new ArgumentNullException(nameof(documentClientFactory));
            _databaseProvider = databaseProvider ?? throw new ArgumentNullException(nameof(databaseProvider));
            _documentClient = documentClientFactory.GetClient();
        }

        public async Task<DocumentCollection> CreateOrGetCollection()
        {
            var collection =
                _documentClient.CreateDocumentCollectionQuery(
                    databaseLink: await _databaseProvider.GetDbSelfLink()
                )
                .Where(c => c.Id == GetCollectionId())
                .AsEnumerable()
                .FirstOrDefault();

            return collection ??
                 await _documentClient.CreateDocumentCollectionAsync(
                    await _databaseProvider.GetDbSelfLink(),
                    new DocumentCollection { Id = GetCollectionId() });
        }

        public virtual async Task<string> GetCollectionDocumentsLink()
        {
            return (await CreateOrGetCollection()).DocumentsLink;
        }

        public virtual string GetCollectionId()
        {
            return typeof(TDocument).Name;
        }
    }
}
