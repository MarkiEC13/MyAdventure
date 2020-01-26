using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Adventure.Persistence
{
    public interface IGenericRepository<TDocument>
    {
        Task<IOrderedQueryable<TDocument>> CreateDocumentQuery(string partitionKey = "");

        Task<IOrderedQueryable<TDocument>> GetAll(string partitionKey = "");

        Task<IQueryable<TDocument>> GetWhere(Expression<Func<TDocument, bool>> predicate, string partitionKey = "");

        Task<TDocument> Get(Expression<Func<TDocument, bool>> predicate, string partitionKey = "");

        Task<TDocument> Get(string id);

        Task<int> GetRecordCount();

        Task<TDocument> GetLastDocument();

        Task<TDocument> Create(TDocument document);

        Task<TDocument> Update(TDocument document);

        Task Delete(TDocument document);
    }
}
