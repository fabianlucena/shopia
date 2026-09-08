using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.QueryOptions;
using RFIServices.IServices;

namespace backend_shopia.IServices;

public interface ICommerceFileService
    : ICreatableWithNameEntityService<CommerceFile>
{
    Task<IEnumerable<CommerceFile>> AddByCommerceUuidAsync(Guid commerceUuid, FilesCollectionDTO files);
    Task<IEnumerable<CommerceFile>> AddByCommerceIdAsync(long commerceId, FilesCollectionDTO files);
    Task<IEnumerable<CommerceFile>> GetListByCommerceIdAsync(long commerceId);
    Task<int> GetCountByOwnerIdAsync(long ownerId, CommerceFileQueryOptions? options = null);
    Task<int> GetCountByCurrentUserAsync(CommerceFileQueryOptions? options = null);
    Task<long> GetAggregatedSizeByOwnerIdAsync(long ownerId, CommerceFileQueryOptions? options = null);
    Task<long> GetAggregatedSizeByCurrentUserAsync(CommerceFileQueryOptions? options = null);
}
