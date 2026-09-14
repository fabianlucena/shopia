using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IRepositories;
using backend_shopia.IServices;
using backend_shopia.QueryOptions;
using RFBase.ILibs;
using RFBase.Libs;
using RFIServices.QueryOptions;
using RFRegisterService.Attributes;
using RFServices.Services;
using System.Text.Json;

namespace backend_shopia.Services;

[RegisterService]
public class ItemService(
    IItemRepository itemRepository,
    IServiceProvider serviceProvider
)
    : ANominableEntityService<Item>(itemRepository, serviceProvider),
    IItemService
{
    public override async Task<Item> ValidateForCreateAsync(Item data)
    {
        data = await base.ValidateForCreateAsync(data);

        if (string.IsNullOrWhiteSpace(data.Name))
            throw new NoNameException();

        if (data.Stores == null || !data.Stores.Any())
            throw new NoStoreProvidedException();

        var stores = data.Stores.ToList();
        IStoreService storeService = ServiceProvider.GetRequiredService<IStoreService>();
        for (var i = 0; i < stores.Count; i++)
        {
            var store = stores[i]
                ?? throw new StoreDoesNotExistException();

            if (store.Commerce == null)
            {
                if (store.CommerceId > 0)
                {
                    var commerceService = ServiceProvider.GetRequiredService<ICommerceService>();
                    stores[0].Commerce = await commerceService.GetSingleOrDefaultByIdAsync(
                            store.CommerceId,
                            new CommerceQueryOptions { IncludeInactive = true }
                        )
                        ?? throw new CommerceDoesNotExistException();
                }
                else if (store.Id > 0)
                {
                    stores[0] = await storeService.GetSingleOrDefaultByIdAsync(
                        store.Id,
                        new StoreQueryOptions
                        {
                            JoinCommerce = true,
                            IncludeInactive = true,
                        }
                    )
                    ?? throw new StoreDoesNotExistException();
                }
                else if (store.Uuid != Guid.Empty)
                {
                    stores[0] = await storeService.GetSingleOrDefaultByUuidAsync(
                        store.Uuid,
                        new StoreQueryOptions
                        {
                            JoinCommerce = true,
                            IncludeInactive = true,
                        }
                    )
                    ?? throw new StoreDoesNotExistException();
                }
                else
                {
                    throw new StoreDoesNotExistException();
                }
            }

            if (data.CommerceId <= 0)
            {
                data.CommerceId = store.CommerceId;
            }
            else if (data.CommerceId != store.CommerceId)
            {
                throw new TheStoreBelongsToAnotherCommerceException();
            }
        }
        data.Stores = stores;

        var userPlanService = ServiceProvider.GetRequiredService<IUserPlanService>();
        var limits = await userPlanService.GetLimitsByCurrentUserAsync();

        var totalItemsCount = await GetCountByCurrentUserAsync(new ItemQueryOptions { IncludeInactive = true });
        if (totalItemsCount >= limits[PlanLimitName.MaxTotalItems])
            throw new TotalItemsLimitReachedException();

        var activeItemsCount = await GetCountByCurrentUserAsync();
        var activeItemsMax = limits[PlanLimitName.MaxEnabledItems];
        if (data.IsActive && activeItemsCount >= activeItemsMax
            || activeItemsCount > activeItemsMax
        )
        {
            throw new MaxActiveItemsLimitReachedException();
        }

        data.InheritedIsActive = data.Stores.Any(s => s.IsActive && (s.Commerce?.IsActive ?? false));

        data.Embedding = await GetEmbedding(data);

        return data;
    }

    public async Task<IEnumerable<Item>> GetListAsync(ItemQueryOptions options)
    {
        var items = await base.GetListAsync(options);
        if (items.Any())
        {
            if (options.IncludeStores)
            {
                var itemStoreOptions = new ItemStoreQueryOptions { IncludeStore = true };

                var itemStoreService = ServiceProvider.GetRequiredService<IItemStoreService>();
                foreach (var item in items)
                {
                    var itemsStores = await itemStoreService.GetListByItemIdAsync(
                        item.Id,
                        itemStoreOptions
                    );
                    if (!itemsStores.Any())
                        continue;

                    item.ItemsStores = itemsStores;
                    item.Stores = itemsStores.Select(i => {
                        i.Store!.Commerce = i.Commerce;  
                        return i.Store;
                    });

                    if (item.CommerceId <= 0)
                        item.CommerceId = item.Stores.First().CommerceId;

                    item.Commerce ??= item.Stores.First().Commerce;
                }
            }
        }

        return items;
    }

    public async Task<float[]> GetEmbedding(Item data)
    {
        if (data.Stores == null || !data.Stores.Any())
        {
            var itemStoreService = ServiceProvider.GetRequiredService<IItemStoreService>();
            data.Stores = await itemStoreService.GetStoresByItemIdAsync(
                    data.Id,
                    new ItemStoreQueryOptions { IncludeInactive = true }
                )
                ?? throw new StoreDoesNotExistException();
        }

        var stores = data.Stores.ToList();
        for (var i = 0; i < stores.Count; i++)
        {
            var store = stores[i]
                ?? throw new StoreDoesNotExistException();

            if (store.Commerce == null)
            {
                var commerceService = ServiceProvider.GetRequiredService<ICommerceService>();
                stores[i].Commerce = await commerceService.GetSingleOrDefaultByIdAsync(
                        store.CommerceId,
                        new CommerceQueryOptions { IncludeInactive = true }
                    )
                    ?? throw new CommerceDoesNotExistException();
            }
        }

        data.Stores = stores;

        var embeddingData = new
        {
            data.Name,
            data.Description,
            data.Category,
            Stores = data.Stores?.Select(s => s.Name),
            Commerce = data.Stores?.First()?.Commerce?.Name,
            data.Price,
            data.IsPresent,
            data.MinAge,
            data.MaxAge,
        };
        var embeddingService = ServiceProvider.GetRequiredService<IEmbeddingService>();
        var embedding = await embeddingService.GetAsync(JsonSerializer.Serialize(embeddingData));

        return embedding;
    }

    public override async Task<Item> CreateAsync(Item item)
    {
        var created = await base.CreateAsync(item);
        if (item.Stores != null)
        {
            IStoreService storeService = ServiceProvider.GetRequiredService<IStoreService>();

            var storeOptions = new StoreQueryOptions { IncludeInactive = true };
            var storesId = new List<long>();
            foreach (var store in item.Stores)
            {
                if (store is null)
                    throw new StoreDoesNotExistException();

                if (store.Id >= 0)
                {
                    storesId.Add(store.Id);
                    continue;
                }

                if (store.Uuid != Guid.Empty)
                {
                    storesId.Add(await storeService.GetSingleIdByUuidAsync(store.Uuid, storeOptions));
                    continue;
                }

                throw new StoreDoesNotExistException();
            }

            IItemStoreService itemStoreService = ServiceProvider.GetRequiredService<IItemStoreService>();
            foreach (var storeId in storesId)
            {
                await itemStoreService.CreateAsync(new ItemStore
                {
                    ItemId = item.Id,
                    StoreId = storeId,
                });
            }
        }

        var getOptions = new ItemQueryOptions
        {
            IncludeInactive = true,
            IncludeCommerce = true,
            IncludeStores = true,
        };
        var embeddingItem = await GetSingleByIdAsync(item.Id, getOptions);
        var embedding = await GetEmbedding(embeddingItem);
        await base.UpdateByIdAsync(
            embeddingItem.Id,
            new DataDictionary { { "Embedding", embedding } }
        );

        return created;
    }

    public override async Task<IDataDictionary> ValidateForUpdateAsync(IDataDictionary data, BaseQueryOptions options)
    {
        data = await base.ValidateForUpdateAsync(data, options);

        if (data.TryGetInt64("CommerceId", out var commerceId))
        {
            if (commerceId <= 0)
                throw new NoCommerceException();
        }

        if (data.TryGetGuid("CommerceUuid", out var commerceUuid))
        {
            if (commerceUuid == Guid.Empty)
                throw new NoCommerceException();

            if (commerceId > 0)
            {
                var commerceService = ServiceProvider.GetRequiredService<ICommerceService>();
                var commerce = await commerceService.GetSingleByUuidAsync(commerceUuid);
                if (commerce.Id != commerceId)
                    throw new IncomptatibleCommerceUUIDdAndIDException();
            }
            else
            {
                var commerceService = ServiceProvider.GetRequiredService<ICommerceService>();
                var commerce = await commerceService.GetSingleByUuidAsync(commerceUuid);
                commerceId = commerce.Id;
            }
        }

        if (data.TryGetGuids("StoresUuid", out var storesUuids))
        {
            if (storesUuids == null || !storesUuids.Any())
                throw new NoStoreProvidedException();

            var storeService = ServiceProvider.GetRequiredService<IStoreService>();
            foreach (var storeUuid in storesUuids)
            {
                var store = await storeService.GetSingleOrDefaultByUuidAsync(
                    storeUuid,
                    new StoreQueryOptions { IncludeInactive = true }
                ) ?? throw new SomeStoreDoesNotExistException();

                if (commerceId == 0)
                    commerceId = store.CommerceId;
                else if (store.CommerceId != commerceId)
                    throw new SomeStoreBelongsToAnotherCommerceException();
            }
        }

        if (data.GetBool("IsActive"))
        {
            var getOptions = new ItemQueryOptions { IncludeInactive = true };
            _ = await GetSingleOrDefaultAsync(getOptions)
                ?? throw new ItemDoesNotExistException();

            var userPlanService = ServiceProvider.GetRequiredService<IUserPlanService>();
            var limits = await userPlanService.GetLimitsByCurrentUserAsync();

            var enabledItemsCount = await GetCountByCurrentUserAsync();
            var enabledItemsMax = limits[PlanLimitName.MaxEnabledItems];
            if (enabledItemsCount >= enabledItemsMax)
                throw new MaxActiveItemsLimitReachedException();
        }

        return data;
    }

    public override async Task<int> UpdateAsync(IDataDictionary data, BaseQueryOptions options)
    {
        var updatedRows = await base.UpdateAsync(data, options);
        if (updatedRows > 0)
        {
            var getOptions = new ItemQueryOptions(options as ItemQueryOptions)
            {
                IncludeInactive = true,
                IncludeCommerce = true,
                IncludeStores = true,
            };
            var items = await GetListAsync(getOptions);

            if (data.TryGetGuids("StoresUuid", out var storesUuid))
            {
                IStoreService storeService = ServiceProvider.GetRequiredService<IStoreService>();
                var storesId = await storeService.GetListIdByUuidAsync(
                    storesUuid,
                    new StoreQueryOptions { IncludeInactive = true }
                );

                foreach (var item in items)
                {
                    var currentStoresIds = item.Stores?.Select(s => s.Id) ?? [];
                    var storesToAdd = storesId.Except(currentStoresIds);
                    var storesToRemove = currentStoresIds.Except(storesId);

                    IItemStoreService itemStoreService = ServiceProvider.GetRequiredService<IItemStoreService>();
                    foreach (var storeId in storesToAdd)
                    {
                        await itemStoreService.CreateAsync(new ItemStore
                        {
                            ItemId = item.Id,
                            StoreId = storeId,
                        });
                    }

                    if (storesToRemove.Any())
                    {
                        await itemStoreService.DeleteAsync(
                            new ItemStoreQueryOptions
                            {
                                ItemId = item.Id,
                                StoresId = storesToRemove,
                            }
                        );
                    }
                }
            }

            foreach (var item in items)
            {
                var embedding = await GetEmbedding(item);
                await base.UpdateByIdAsync(
                    item.Id,
                    new DataDictionary { { "Embedding", embedding } }
                );
            }

            if (data.TryGetDecimal("Price", out var price))
            {
                var itemPriceLogData = new ItemPriceLog
                {
                    Price = price,
                };
                IItemPriceLogService itemPriceLogService = ServiceProvider.GetRequiredService<IItemPriceLogService>();
                foreach (var item in items)
                {
                    itemPriceLogData.ItemId = item.Id;
                    await itemPriceLogService.CreateAsync(itemPriceLogData);
                }
            }
        }

        return updatedRows;
    }

    public async Task<bool> CheckByUuidAndCurrentUserAsync(Guid uuid, ItemQueryOptions? options = null)
    {
        options = options?.Clone() ?? new ItemQueryOptions();
        options.IncludeInactive = true;
        options.Uuid = uuid;
        var itemId = await GetSingleIdOrDefaultAsync(options)
            ?? throw new ItemDoesNotExistException();

        var storeService = ServiceProvider.GetRequiredService<IStoreService>();
        var storesId = await storeService.GetListIdByCurrentUserAsync(new StoreQueryOptions { IncludeInactive = true });
        if (!storesId.Any())
            throw new ItemDoesNotExistException();

        var itemStoreService = ServiceProvider.GetRequiredService<IItemStoreService>();
        var itemStoreOptions = new ItemStoreQueryOptions
        {
            ItemId = itemId,
            StoresId = storesId,
        };
        _ = await itemStoreService.GetFirstOrDefaultAsync(itemStoreOptions)
            ?? throw new ItemDoesNotExistException();

        return true;
    }

    public async Task<ItemQueryOptions> GetFilterByOwnerIdAsync(long ownerId, ItemQueryOptions? options = null)
    {
        var commerceService = ServiceProvider.GetRequiredService<ICommerceService>();
        var commercesId = await commerceService.GetListIdByOwnerIdAsync(ownerId, new CommerceQueryOptions { IncludeInactive = options?.IncludeInactive ?? false });

        options = options?.Clone() ?? new ItemQueryOptions(options);
        options.IncludeCommerce = true;
        options.CommercesId = commercesId;

        return options;
    }

    public async Task<int> GetCountByOwnerIdAsync(long ownerId, ItemQueryOptions? options = null)
        => await GetCountAsync(await GetFilterByOwnerIdAsync(ownerId, options));

    public async Task<int> GetCountByCurrentUserAsync(ItemQueryOptions? options = null)
        => await GetCountByOwnerIdAsync(await GetCurrentUserIdAsync(), options);

    public async Task<IEnumerable<long>> GetListIdByOwnerIdAsync(long ownerId, ItemQueryOptions? options = null)
        => await GetListIdAsync(await GetFilterByOwnerIdAsync(ownerId, options));

    public async Task<IEnumerable<long>> GetListIdByCurrentUserAsync(ItemQueryOptions? options = null)
        => await GetListIdByOwnerIdAsync(await GetCurrentUserIdAsync(), options);

    public async Task<IEnumerable<Guid>> GetListUuidByCurrentUserAsync(ItemQueryOptions? options = null)
        => await GetListUuidAsync(await GetFilterByOwnerIdAsync(await GetCurrentUserIdAsync(), options));

    public async Task<int> UpdateInheritedByUuidAsync(Guid uuid)
        => await itemRepository.UpdateInheritedByUuidAsync(uuid);

    public async Task<int> UpdateInheritedByStoreUuidAsync(Guid storeUuid)
        => await itemRepository.UpdateInheritedByStoreUuidAsync(storeUuid);

    public async Task<int> UpdateInheritedByCommerceUuidAsync(Guid commerceUuid)
        => await itemRepository.UpdateInheritedByCommerceUuidAsync(commerceUuid);
}