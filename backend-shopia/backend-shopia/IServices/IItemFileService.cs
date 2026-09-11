using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.QueryOptions;
using RFIServices.IServices;

namespace backend_shopia.IServices;

public interface IItemFileService
    : ICreatableWithNameEntityService<ItemFile>
{
    Task<IEnumerable<ItemFile>> AddByItemUuidAsync(Guid itemUuid, FilesCollectionDTO files);

    Task<IEnumerable<ItemFile>> AddByItemIdAsync(long itemId, FilesCollectionDTO files);

    Task<IEnumerable<ItemFile>> GetListByItemIdAsync(long itemId);

    Task<int> GetCountByOwnerIdAsync(long ownerId, ItemFileQueryOptions? options = null);

    Task<int> GetCountByCurrentUserAsync(ItemFileQueryOptions? options = null);

    Task<long> GetAggregatedSizeByOwnerIdAsync(long ownerId, ItemFileQueryOptions? options = null);

    Task<long> GetAggregatedSizeByCurrentUserAsync(ItemFileQueryOptions? options = null);
}
