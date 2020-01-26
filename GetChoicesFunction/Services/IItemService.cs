using Adventure.Persistence.Containers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GetChoicesFunction.Services
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetRoot();

        Task<IEnumerable<Item>> Get(string parentId);
    }
}
