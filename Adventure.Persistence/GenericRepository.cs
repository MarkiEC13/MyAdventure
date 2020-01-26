using Adventure.Persistence.Providers;
using Microsoft.Azure.Documents;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Adventure.Persistence
{
    public class GenericRepository<TDocument> : IGenericRepository<TDocument> where TDocument : Resource
    {
        private readonly IDocumentClient _documentClient;
        private readonly ICollectionProvider _collectionProvider;

        /// <summary>
        /// TODO: GenericCollectionProvider register in DI
        /// </summary>
        /// <param name="documentClientFactory"></param>
        /// <param name="databaseProvider"></param>
        public GenericRepository(IDocumentClientFactory documentClientFactory, IDatabaseProvider databaseProvider) : this(
            documentClientFactory.GetClient(),
            new GenericCollectionProvider<TDocument>(
                documentClientFactory,
                databaseProvider))
        {
        }

        public GenericRepository(
            IDocumentClient documentClient,
            ICollectionProvider collectionProvider)
        {
            _documentClient = documentClient ?? throw new ArgumentNullException(nameof(documentClient));
            _collectionProvider = collectionProvider ?? throw new ArgumentNullException(nameof(collectionProvider));
        }

        public virtual async Task<IOrderedQueryable<TDocument>> CreateDocumentQuery(string partitionKey = "")
        {
            return _documentClient.CreateDocumentQuery<TDocument>(
                await _collectionProvider.GetCollectionDocumentsLink(),
                new Microsoft.Azure.Documents.Client.FeedOptions
                {
                    PartitionKey = !string.IsNullOrEmpty(partitionKey) ? new PartitionKey(partitionKey) : null
                });
        }

        public virtual async Task<IOrderedQueryable<TDocument>> GetAll(string partitionKey = "")
        {
            return await CreateDocumentQuery(partitionKey);
        }

        public virtual async Task<IQueryable<TDocument>> GetWhere(Expression<Func<TDocument, bool>> predicate, string partitionKey = "")
        {
            return (await CreateDocumentQuery(partitionKey)).Where(predicate);
        }

        public virtual async Task<TDocument> Get(Expression<Func<TDocument, bool>> predicate, string partitionKey = "")
        {
            return (await GetWhere(predicate, partitionKey))
                .AsEnumerable()
                .FirstOrDefault();
        }

        public virtual async Task<TDocument> Get(string id)
        {
            return await Get(d => d.Id == id);
        }

        public virtual async Task<int> GetRecordCount()
        {
            var documentList = await GetAll();
            var documentCount = 0;
            TDocument lastDocument = documentList.AsEnumerable().LastOrDefault();

            foreach (var document in documentList.AsEnumerable())
            {
                documentCount++;
                lastDocument = document;
            }
            // var lastDocument = documentList.LastOrDefault();
            return documentCount;
        }

        public virtual async Task<TDocument> GetLastDocument()
        {
            var documentList = await GetAll();
            TDocument lastDocument = documentList.AsEnumerable().LastOrDefault();
            return lastDocument;
        }

        public virtual async Task<TDocument> Create(TDocument document)
        {
            return (TDocument)(dynamic)(await _documentClient.CreateDocumentAsync(
                await _collectionProvider.GetCollectionDocumentsLink(),
                document))
                .Resource;
        }

        public virtual async Task<TDocument> Update(TDocument document)
        {
            var selfLink = await GetDocumentSelfLink(document);

            if (string.IsNullOrWhiteSpace(selfLink))
            {
                throw new InvalidOperationException("Document does not exist in collection");
            }

            return (TDocument)(dynamic)(await _documentClient.ReplaceDocumentAsync(
                selfLink,
                document))
                .Resource;
        }

        public virtual async Task Delete(TDocument document)
        {
            var selfLink = await GetDocumentSelfLink(document);

            if (string.IsNullOrWhiteSpace(selfLink))
            {
                throw new InvalidOperationException("Document does not exist in collection");
            }

            await _documentClient.DeleteDocumentAsync(selfLink);
        }

        protected virtual async Task<string> GetDocumentSelfLink(TDocument document)
        {
            return string.IsNullOrWhiteSpace(document.SelfLink)
                                  ? (await Get(document.Id))?.SelfLink
                                  : document.SelfLink;
        }
    }
}