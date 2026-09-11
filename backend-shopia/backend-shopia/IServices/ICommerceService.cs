using backend_shopia.Entities;
using backend_shopia.QueryOptions;
using RFIServices.IServices;

namespace backend_shopia.IServices;

public interface ICommerceService
    : IANominableOwnedEntityService<Commerce>, IGetCurrentAndSystemUserService
{
    Task<bool> CheckByUuidAndCurrentUserAsync(Guid uuid, CommerceQueryOptions? options = null);

    CommerceQueryOptions GetFilterByOwnerIdAsync(long ownerId, CommerceQueryOptions? options = null);

    Task<int> GetCountByOwnerIdAsync(long ownerId, CommerceQueryOptions? options = null);

    Task<int> GetCountByCurrentUserAsync(CommerceQueryOptions? options = null);

    Task<IEnumerable<long>> GetListIdByOwnerIdAsync(long ownerId, CommerceQueryOptions? options = null);

    Task<IEnumerable<Guid>> GetListUuidByOwnerIdAsync(long ownerId, CommerceQueryOptions? options = null);

    Task<IEnumerable<long>> GetListIdByCurrentUserAsync(CommerceQueryOptions? options = null);

    Task<IEnumerable<Guid>> GetListUuidByCurrentUserAsync(CommerceQueryOptions? options = null);
}
