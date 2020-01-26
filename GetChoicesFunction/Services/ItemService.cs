using Adventure.Core.Exceptions;
using Adventure.Persistence;
using Adventure.Persistence.Containers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetChoicesFunction.Services
{
    public class ItemService : IItemService
    {
        private readonly IGenericRepository<Doughnut> _genericRepository;

        public ItemService(IGenericRepository<Doughnut> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<IEnumerable<Item>> GetRoot()
        {
            var items = new List<Item>();
            var rootNode = (await _genericRepository.GetWhere(i => i.ParenId == null))
                ?.AsEnumerable()?.FirstOrDefault();
            if(rootNode == null)
                throw new ItemNotFoundException("Root node not found");
            items.Add(rootNode);

            return items;
        }

        public async Task<IEnumerable<Item>> Get(string parentId)
        {
            return await _genericRepository.GetAll(parentId);
        }
    }
}
