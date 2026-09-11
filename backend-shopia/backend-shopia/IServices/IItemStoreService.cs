using backend_shopia.Entities;
using backend_shopia.QueryOptions;
using RFIServices.IServices;

namespace backend_shopia.IServices;

public interface IItemStoreService
    : ICreatableJoinService<ItemStore>
{
    Task<IEnumerable<ItemStore>> GetListByItemIdAsync(long itemId, ItemStoreQueryOptions? options = null);
    Task<IEnumerable<Store>> GetStoresByItemIdAsync(long itemId, ItemStoreQueryOptions? options = null);
}
