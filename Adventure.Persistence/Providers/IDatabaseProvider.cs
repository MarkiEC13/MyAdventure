using Microsoft.Azure.Documents;
using System.Threading.Tasks;

namespace Adventure.Persistence.Providers
{
    public interface IDatabaseProvider
    {
        Task<Database> CreateOrGetDb();

        Task<string> GetDbSelfLink();
    }
}
