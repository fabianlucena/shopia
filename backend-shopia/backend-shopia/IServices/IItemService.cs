using backend_shopia.Entities;
using backend_shopia.QueryOptions;
using RFIServices.IServices;

namespace backend_shopia.IServices;

public interface IItemService
    : INominableEntityService<Item>, IGetCurrentAndSystemUserService
{
    Task<IEnumerable<Item>> GetListAsync(ItemQueryOptions options);
    Task<bool> CheckByUuidAndCurrentUserAsync(Guid uuid, ItemQueryOptions? options = null);
    Task<ItemQueryOptions> GetFilterByOwnerIdAsync(long ownerId, ItemQueryOptions? options = null);
    Task<int> GetCountByOwnerIdAsync(long ownerId, ItemQueryOptions? options = null);
    Task<int> GetCountByCurrentUserAsync(ItemQueryOptions? options = null);
    Task<IEnumerable<long>> GetListIdByOwnerIdAsync(long ownerId, ItemQueryOptions? options = null);
    Task<IEnumerable<long>> GetListIdByCurrentUserAsync(ItemQueryOptions? options = null);
    Task<IEnumerable<Guid>> GetListUuidByCurrentUserAsync(ItemQueryOptions? options = null);

    Task<int> UpdateInheritedByUuidAsync(Guid uuid, ItemQueryOptions? options = null);
    Task<int> UpdateInheritedByStoreUuidAsync(Guid storeUuid, ItemQueryOptions? options = null);
    Task<int> UpdateInheritedByCommerceUuidAsync(Guid commerceUuid, ItemQueryOptions? options = null);
}
