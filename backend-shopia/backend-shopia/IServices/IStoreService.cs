using backend_shopia.Entities;
using backend_shopia.QueryOptions;
using RFIServices.IServices;

namespace backend_shopia.IServices;

public interface IStoreService
    : IANominableEntityService<Store>
{
    Task<bool> CheckByUuidAndCurrentUserAsync(Guid uuid, StoreQueryOptions? options = null);
    Task<StoreQueryOptions> GetFilterByOwnerIdAsync(Int64 ownerId, StoreQueryOptions? options = null);
    Task<int> GetCountByOwnerIdAsync(Int64 ownerId, StoreQueryOptions? options = null);
    Task<int> GetCountByCurrentUserAsync(StoreQueryOptions? options = null);
    Task<IEnumerable<Int64>> GetListIdByOwnerIdAsync(Int64 ownerId, StoreQueryOptions? options = null);
    Task<IEnumerable<Int64>> GetListIdByCurrentUserAsync(StoreQueryOptions? options = null);
}
