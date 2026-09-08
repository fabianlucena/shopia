using backend_shopia.Entities;
using backend_shopia.QueryOptions;
using RFIServices.IServices;

namespace backend_shopia.IServices;

public interface IItemStoreService
    : ICreatableEntityService<ItemStore>
{
    Task<IEnumerable<ItemStore>> GetListByItemIdAsync(long itemId, ItemStoreQueryOptions? options = null);
    Task<IEnumerable<Store>> GetListStoresByItemIdAsync(long itemId, ItemStoreQueryOptions? options = null);
}
