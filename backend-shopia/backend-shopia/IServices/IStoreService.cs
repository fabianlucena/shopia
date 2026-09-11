using backend_shopia.Entities;
using backend_shopia.QueryOptions;
using RFIServices.IServices;

namespace backend_shopia.IServices;

public interface IStoreService
    : IANominableEntityService<Store>
{
    Task<bool> CheckByUuidAndCurrentUserAsync(Guid uuid, StoreQueryOptions? options = null);
    Task<StoreQueryOptions> GetFilterByOwnerIdAsync(long ownerId, StoreQueryOptions? options = null);
    Task<int> GetCountByOwnerIdAsync(long ownerId, StoreQueryOptions? options = null);
    Task<int> GetCountByCurrentUserAsync(StoreQueryOptions? options = null);
    Task<IEnumerable<long>> GetListIdByOwnerIdAsync(long ownerId, StoreQueryOptions? options = null);
    Task<IEnumerable<long>> GetListIdByCurrentUserAsync(StoreQueryOptions? options = null);
    Task<IEnumerable<Store>> GetListByUuidsAsync(IEnumerable<Guid> uuids, StoreQueryOptions? options = null);
}
