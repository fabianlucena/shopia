using backend_shopia.Entities;
using backend_shopia.QueryOptions;
using RFIServices.IServices;

namespace backend_shopia.IServices;

public interface ICommerceService
    : IANominableOwnedEntityService<Commerce>
{
    //Task<bool> CheckByUuidAndCurrentUserAsync(Guid uuid, CommerceQueryOptions? options = null);

    CommerceQueryOptions GetFilterByOwnerIdAsync(Int64 ownerId, CommerceQueryOptions? options = null);

    Task<int> GetCountByOwnerIdAsync(Int64 ownerId, CommerceQueryOptions? options = null);

    Task<int> GetCountByCurrentUserAsync(CommerceQueryOptions? options = null);

    Task<IEnumerable<Int64>> GetListIdByOwnerIdAsync(Int64 ownerId, CommerceQueryOptions? options = null);

    Task<IEnumerable<Guid>> GetListUuidByOwnerIdAsync(Int64 ownerId, CommerceQueryOptions? options = null);

    Task<IEnumerable<Int64>> GetListIdByCurrentUserAsync(CommerceQueryOptions? options = null);

    Task<IEnumerable<Guid>> GetListUuidByCurrentUserAsync(CommerceQueryOptions? options = null);
}
