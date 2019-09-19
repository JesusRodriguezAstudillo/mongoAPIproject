using System.Collections.Generic;
using System.Threading.Tasks;

namespace mongoAPI.Models
{
    public interface IItemRepository
    {
        Task Add(Item item);
        Task Update(Item item);
        Task Delete(string itemId);
        Task<Item> GetItem(string id);
        Task<Item> GetItem(string name, bool byName);
        Task<IEnumerable<Item>> GetItems();
    }
}