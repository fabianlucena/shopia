using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.QueryOptions;
using RFIServices.IServices;

namespace backend_shopia.IServices;

public interface IItemFileService
    : INominableEntityService<ItemFile>
{
    Task<IEnumerable<ItemFile>> AddForItemUuidAsync(Guid itemUuid, FilesCollectionDTO files);

    Task<IEnumerable<ItemFile>> AddForItemIdAsync(Int64 itemId, FilesCollectionDTO files);

    Task<IEnumerable<ItemFile>> GetListForItemIdAsync(Int64 itemId);

    Task<int> GetCountForOwnerIdAsync(Int64 ownerId, ItemFileQueryOptions? options = null);

    Task<int> GetCountForCurrentUserAsync(ItemFileQueryOptions? options = null);

    Task<Int64> GetAggregatedSizeForOwnerIdAsync(Int64 ownerId, ItemFileQueryOptions? options = null);

    Task<Int64> GetAggregatedSizeForCurrentUserAsync(ItemFileQueryOptions? options = null);
}
