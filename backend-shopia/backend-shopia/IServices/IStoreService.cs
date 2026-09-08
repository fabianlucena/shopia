using backend_shopia.Entities;
using backend_shopia.QueryOptions;
using RFIServices.IServices;

namespace backend_shopia.IServices;

public interface IStoreService
    : IANominableEntityService<Store>
{
    Task<bool> CheckForUuidAndCurrentUserAsync(Guid uuid, StoreQueryOptions? options = null);
    Task<StoreQueryOptions> GetFilterForOwnerIdAsync(Int64 ownerId, StoreQueryOptions? options = null);
    Task<int> GetCountForOwnerIdAsync(Int64 ownerId, StoreQueryOptions? options = null);
    Task<int> GetCountForCurrentUserAsync(StoreQueryOptions? options = null);
    Task<IEnumerable<Int64>> GetListIdForOwnerIdAsync(Int64 ownerId, StoreQueryOptions? options = null);
    Task<IEnumerable<Int64>> GetListIdForCurrentUserAsync(StoreQueryOptions? options = null);
}
