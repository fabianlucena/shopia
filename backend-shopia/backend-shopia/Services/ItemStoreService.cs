using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IRepositories;
using backend_shopia.IServices;
using backend_shopia.QueryOptions;
using RFServices.Services;

namespace backend_shopia.Services;

public class ItemStoreService(
    IItemStoreRepository itemStoreRepository,
    IServiceProvider serviceProvider
)
    : CreatableJoinService<ItemStore>(itemStoreRepository, serviceProvider),
    IItemStoreService
{
    public override async Task<ItemStore> ValidateForCreateAsync(ItemStore data)
    {
        data = await base.ValidateForCreateAsync(data);

        if (data.ItemId <= 0)
        {
            data.ItemId = data.Item?.Id ?? 0;
            if (data.ItemId <= 0)
                throw new NoItemException();
        }

        if (data.StoreId <= 0)
        {
            data.StoreId = data.Store?.Id ?? 0;
            if (data.StoreId <= 0)
                throw new NoStoreProvidedException();
        }

        return data;
    }

    public async Task<IEnumerable<ItemStore>> GetListByItemIdAsync(long itemId, ItemStoreQueryOptions? options = null)
    {
        options = options?.Clone() ?? new ItemStoreQueryOptions();
        options.ItemId = itemId;
        return await GetListAsync(options);
    }

    public async Task<IEnumerable<Store>> GetStoresByItemIdAsync(long itemId, ItemStoreQueryOptions? options = null)
    {
        options = options?.Clone() ?? new ItemStoreQueryOptions();
        options.IncludeStore = true;

        return (await GetListByItemIdAsync(itemId, options))
            .Select(i => i.Store!);
    }
}