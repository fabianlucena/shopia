using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.QueryOptions;
using RFIServices.IServices;

namespace backend_shopia.IServices;

public interface IItemFileService
    : ICreatableWithNameEntityService<ItemFile>
{
    Task<IEnumerable<ItemFile>> AddByItemUuidAsync(Guid itemUuid, FilesCollectionDTO files);

    Task<IEnumerable<ItemFile>> AddByItemIdAsync(Int64 itemId, FilesCollectionDTO files);

    Task<IEnumerable<ItemFile>> GetListByItemIdAsync(Int64 itemId);

    Task<int> GetCountByOwnerIdAsync(Int64 ownerId, ItemFileQueryOptions? options = null);

    Task<int> GetCountByCurrentUserAsync(ItemFileQueryOptions? options = null);

    Task<Int64> GetAggregatedSizeByOwnerIdAsync(Int64 ownerId, ItemFileQueryOptions? options = null);

    Task<Int64> GetAggregatedSizeByCurrentUserAsync(ItemFileQueryOptions? options = null);
}
