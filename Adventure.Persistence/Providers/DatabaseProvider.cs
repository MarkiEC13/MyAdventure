using Microsoft.Azure.Documents;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure.Persistence.Providers
{
    public class DatabaseProvider : IDatabaseProvider
    {
        private readonly string _databaseId;
        private readonly IDocumentClient _documentClient;        

        public DatabaseProvider(IDocumentClientFactory documentClientFactory)
        {
            if (documentClientFactory == null) throw new ArgumentNullException(nameof(documentClientFactory));

            _documentClient = documentClientFactory.GetClient();
            _databaseId = documentClientFactory.GetDatabaseId();
        }

        public virtual async Task<Database> CreateOrGetDb()
        {
            var db = this._documentClient.CreateDatabaseQuery()
                .Where(d => d.Id == this._databaseId)
                .AsEnumerable()
                .FirstOrDefault();

            return db ?? await this._documentClient.CreateDatabaseAsync(new Database { Id = this._databaseId });
        }

        public virtual async Task<string> GetDbSelfLink()
        {
            return (await this.CreateOrGetDb()).SelfLink;
        }
    }
}
