using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IServices;
using RFAuth.Exceptions;
using RFOperators;
using RFService.ILibs;
using RFService.IRepo;
using RFService.Libs;
using RFService.Repo;
using RFService.Services;
using System.Text.Json;

namespace backend_shopia.Services
{
    public class ItemService(
        IRepo<Item> repo,
        IServiceProvider serviceProvider
    )
        : ServiceSoftDeleteTimestampsIdUuidEnabledName<IRepo<Item>, Item>(repo),
            IItemService
    {
        public override async Task<Item> ValidateForCreationAsync(Item data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (string.IsNullOrWhiteSpace(data.Name))
                throw new NoNameException();

            if (data.Stores == null || !data.Stores.Any())
                throw new NoStoreProvidedException();

            var stores = data.Stores.ToList();
            IStoreService storeService = serviceProvider.GetRequiredService<IStoreService>();
            for (var i = 0; i < stores.Count; i++)
            {
                var store = stores[i]
                    ?? throw new StoreDoesNotExistException();

                if (store.Commerce == null)
                {
                    if (store.CommerceId > 0)
                    {
                        var commerceService = serviceProvider.GetRequiredService<ICommerceService>();
                        stores[0].Commerce = await commerceService.GetSingleOrDefaultForIdAsync(
                                store.CommerceId,
                                new QueryOptions
                                {
                                    Switches = { { "IncludeDisabled", true } },
                                }
                            )
                            ?? throw new CommerceDoesNotExistException();
                    }
                    else if (store.Id > 0)
                    {
                        stores[0] = await storeService.GetSingleOrDefaultForIdAsync(
                            store.Id,
                            new QueryOptions
                            {
                                Join = { { "Commerce" } },
                                Switches = { { "IncludeDisabled", true } },
                            }
                        )
                        ?? throw new StoreDoesNotExistException();
                    }
                    else if (store.Uuid != Guid.Empty)
                    {
                        stores[0] = await storeService.GetSingleOrDefaultForUuidAsync(
                            store.Uuid,
                            new QueryOptions
                            {
                                Join = { { "Commerce" } },
                                Switches = { { "IncludeDisabled", true } },
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

            var userPlanService = serviceProvider.GetRequiredService<IUserPlanService>();
            var limits = await userPlanService.GetLimitsForCurrentUserAsync();

            var totalItemsCount = await GetCountForCurrentUserAsync(new QueryOptions { Filters = { { "IsEnabled", null } } });
            if (totalItemsCount >= limits[PlanLimitName.MaxTotalItems])
                throw new TotalItemsLimitReachedException();

            var enabledItemsCount = await GetCountForCurrentUserAsync();
            var enabledItemsMax = limits[PlanLimitName.MaxEnabledItems];
            if (data.IsEnabled && enabledItemsCount >= enabledItemsMax
                || enabledItemsCount > enabledItemsMax
            )
            {
                throw new MaxEnabledItemsLimitReachedException();
            }

            data.InheritedIsEnabled = data.Stores.Any(s => s.IsEnabled && (s.Commerce?.IsEnabled ?? false));

            data.Embedding = await GetEmbedding(data);

            return data;
        }

        public override async Task<IEnumerable<Item>> GetListAsync(QueryOptions options)
        {
            var items = await base.GetListAsync(options);
            if (items.Any())
            {
                if (options.Switches.TryGetValue("IncludeStores", out var includeStores)
                    && includeStores)
                {
                    var itemStoreOptions = new QueryOptions();
                    itemStoreOptions.Include("Store", "store");

                    var itemStoreService = serviceProvider.GetRequiredService<IItemStoreService>();
                    foreach (var item in items)
                    {
                        var itemsStores = await itemStoreService.GetListForItemIdAsync(
                            item.Id,
                            new QueryOptions(itemStoreOptions)
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
                var itemStoreService = serviceProvider.GetRequiredService<IItemStoreService>();
                data.Stores = await itemStoreService.GetListStoresForItemIdAsync(
                        data.Id,
                        new QueryOptions
                        {
                            Switches = { { "IncludeDisabled", true } },
                        }
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
                    var commerceService = serviceProvider.GetRequiredService<ICommerceService>();
                    stores[i].Commerce = await commerceService.GetSingleOrDefaultForIdAsync(
                            store.CommerceId,
                            new QueryOptions
                            {
                                Switches = { { "IncludeDisabled", true } },
                            }
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
            var embeddingService = serviceProvider.GetRequiredService<IEmbeddingService>();
            var embedding = await embeddingService.GetAsync(JsonSerializer.Serialize(embeddingData));

            return embedding;
        }

        public override async Task<Item> CreateAsync(Item item, QueryOptions? options)
        {
            var created = await base.CreateAsync(item, options);
            if (item.Stores != null)
            {
                IStoreService storeService = serviceProvider.GetRequiredService<IStoreService>();

                var storesId = new List<Int64>();
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
                        storesId.Add(await storeService.GetSingleIdForUuidAsync(
                            store.Uuid,
                            new QueryOptions
                            {
                                Switches = { { "IncludeDisabled", true } },
                            }
                        ));
                        continue;
                    }

                    throw new StoreDoesNotExistException();
                }

                IItemStoreService itemStoreService = serviceProvider.GetRequiredService<IItemStoreService>();
                foreach (var storeId in storesId)
                {
                    await itemStoreService.CreateAsync(new ItemStore
                    {
                        ItemId = item.Id,
                        StoreId = storeId,
                    });
                }
            }

            var getOptions = new QueryOptions(options);
            getOptions.IncludeIfNotExists("Commerce");
            getOptions.Switches["IncludeDisabled"] = true;
            getOptions.Switches["IncludeStores"] = true;
            var embeddingItem = await GetSingleForIdAsync(item.Id, getOptions);
            var embedding = await GetEmbedding(embeddingItem);
            await base.UpdateAsync(
                new DataDictionary { { "Embedding", embedding } },
                new QueryOptions { Filters = { { "Id", embeddingItem.Id } } }
            );

            return created;
        }

        public override async Task<IDataDictionary> ValidateForUpdateAsync(IDataDictionary data, QueryOptions options)
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
                    var commerceService = serviceProvider.GetRequiredService<ICommerceService>();
                    var commerce = await commerceService.GetSingleForUuidAsync(commerceUuid);
                    if (commerce.Id != commerceId)
                        throw new CommerceDoesNotExistException();
                }
            }

            if (data.TryGetGuids("StoresUuid", out var storesUuids))
            {
                if (storesUuids == null || !storesUuids.Any())
                    throw new NoStoreProvidedException();

                if (commerceId < 0)
                {
                    var item = await GetSingleOrDefaultAsync(options)
                        ?? throw new ItemDoesNotExistException();
                    commerceId = item.CommerceId;
                }

                var storeService = serviceProvider.GetRequiredService<IStoreService>();
                foreach (var storeUuid in storesUuids)
                {
                    var store = await storeService.GetSingleOrDefaultForUuidAsync(
                        storeUuid,
                        QueryOptions.IncludeDisabled
                    ) ?? throw new SomeStoreDoesNotExistException();

                    if (store.CommerceId != commerceId)
                        throw new SomeStoreBelongsToAnotherCommerceException();
                }
            }

            if (data.TryGetValue("IsEnabled", out var isEnabledValue)
                && isEnabledValue is bool isEnabled && isEnabled)
            {
                var getOptions = new QueryOptions(options)
                {
                    Switches = { { "IncludeDisabled", true } }
                };
                _ = await GetSingleOrDefaultAsync(getOptions)
                    ?? throw new ItemDoesNotExistException();

                var userPlanService = serviceProvider.GetRequiredService<IUserPlanService>();
                var limits = await userPlanService.GetLimitsForCurrentUserAsync();

                var enabledItemsCount = await GetCountForCurrentUserAsync();
                var enabledItemsMax = limits[PlanLimitName.MaxEnabledItems];
                if (enabledItemsCount >= enabledItemsMax)
                    throw new MaxEnabledItemsLimitReachedException();
            }

            return data;
        }

        public override async Task<int> UpdateAsync(IDataDictionary data, QueryOptions options)
        {
            var updatedRows = await base.UpdateAsync(data, options);
            if (updatedRows > 0)
            {
                var getOptions = new QueryOptions(options);
                getOptions.IncludeIfNotExists("Commerce");
                getOptions.Switches["IncludeDisabled"] = true;
                getOptions.Switches["IncludeStores"] = true;
                var items = await GetListAsync(getOptions);

                if (data.TryGetGuids("StoresUuid", out var storesUuid))
                {
                    IStoreService storeService = serviceProvider.GetRequiredService<IStoreService>();
                    var storesId = await storeService.GetListIdForUuidsAsync(
                        storesUuid,
                        new QueryOptions
                        {
                            Switches = { { "IncludeDisabled", true } },
                        }
                    );

                    foreach (var item in items)
                    {
                        var currentStoresIds = item.Stores?.Select(s => s.Id) ?? [];
                        var storesToAdd = storesId.Except(currentStoresIds);
                        var storesToRemove = currentStoresIds.Except(storesId);

                        IItemStoreService itemStoreService = serviceProvider.GetRequiredService<IItemStoreService>();
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
                                new QueryOptions
                                {
                                    Filters =
                                    {
                                        { "ItemId", item.Id },
                                        { "StoreId", storesToRemove },
                                    }
                                }
                            );
                        }
                    }
                }

                foreach (var item in items)
                {
                    var embedding = await GetEmbedding(item);
                    await base.UpdateAsync(
                        new DataDictionary { { "Embedding", embedding } },
                        new QueryOptions { Filters = { { "Id", item.Id } } }
                    );
                }

                if (data.TryGetDecimal("Price", out var price))
                {
                    var itemPriceLogData = new ItemPriceLog
                    {
                        Price = price,
                    };
                    IItemPriceLogService itemPriceLogService = serviceProvider.GetRequiredService<IItemPriceLogService>();
                    foreach (var item in items)
                    {
                        itemPriceLogData.ItemId = item.Id;
                        await itemPriceLogService.CreateAsync(itemPriceLogData);
                    }
                }
            }

            return updatedRows;
        }

        public async Task<bool> CheckForUuidAndCurrentUserAsync(Guid uuid, QueryOptions? options = null)
        {
            options = new (options);
            options.Switches["IncludeDisabled"] = true;
            options.AddFilter("Uuid", uuid);
            var itemId = await GetSingleIdOrNullAsync(options)
                ?? throw new ItemDoesNotExistException();

            var storeService = serviceProvider.GetRequiredService<IStoreService>();
            var storesId = await storeService.GetListIdForCurrentUserAsync(QueryOptions.IncludeDisabled);
            if (storesId.Count() == 0)
                throw new ItemDoesNotExistException();

            var itemStoreService = serviceProvider.GetRequiredService<IItemStoreService>();
            var itemStoreOptions = new QueryOptions
            {
                Filters =
                {
                    { "ItemId", itemId },
                    { "StoreId", storesId },
                }
            };
            _ = await itemStoreService.GetFirstOrDefaultAsync(itemStoreOptions)
                ?? throw new ItemDoesNotExistException();

            return true;
        }

        public async Task<QueryOptions> GetFilterForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null)
        {
            var commerceService = serviceProvider.GetRequiredService<ICommerceService>();
            var commercesId = await commerceService.GetListIdForOwnerIdAsync(ownerId, options);

            options = new QueryOptions(options);
            options.IncludeIfNotExists("Commerce", "commerce");
            options.AddFilter("CommerceId", commercesId);

            return options;
        }

        public async Task<int> GetCountForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null)
            => await GetCountAsync(await GetFilterForOwnerIdAsync(ownerId, options));

        public Int64? GetCurrentUserIdOrDefault()
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext == null)
                return null;

            var userId = (httpContext.Items["UserId"] as Int64?);
            if (userId == null || userId <= 0)
                return null;

            return userId!;
        }

        public Int64 GetCurrentUserId()
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new NoAuthorizationHeaderException();

            var userId = (httpContext.Items["UserId"] as Int64?)
                ?? throw new NoSessionUserDataException();

            if (userId <= 0)
                throw new NoSessionUserDataException();

            return userId!;
        }

        public async Task<int> GetCountForCurrentUserAsync(QueryOptions? options = null)
            => await GetCountForOwnerIdAsync(GetCurrentUserId(), options);

        public async Task<IEnumerable<Int64>> GetListIdForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null)
            => await GetListIdAsync(await GetFilterForOwnerIdAsync(ownerId, options));

        public async Task<IEnumerable<Int64>> GetListIdForCurrentUserAsync(QueryOptions? options = null)
            => await GetListIdForOwnerIdAsync(GetCurrentUserId(), options);

        public async Task<IEnumerable<Guid>> GetListUuidForCurrentUserAsync(QueryOptions? options = null)
            => await GetListUuidAsync(await GetFilterForOwnerIdAsync(GetCurrentUserId(), options));

        public (QueryOptions, DataDictionary) GetOptionsForUpdateInherited(QueryOptions? options = null)
        {
            options ??= new();
            options.Include(
                "",
                "itemStore",
                entity: typeof(ItemStore),
                on: Op.Eq("itemStore.ItemId", Op.Column("Id"))
            );
            options.Include(
                "",
                "store",
                entity: typeof(Store),
                on: Op.Eq("store.Id", Op.Column("itemStore.StoreId"))
            );
            options.Include(
                "",
                "commerce",
                entity: typeof(Commerce),
                on: Op.Eq("commerce.Id", Op.Column("store.CommerceId"))
            );

            var data = new DataDictionary
            {
                { "InheritedIsEnabled",
                    Op.And(
                        Op.Eq("store.IsEnabled", true),
                        Op.IsNull("store.DeletedAt"),
                        Op.Eq("commerce.IsEnabled", true),
                        Op.IsNull("commerce.DeletedAt")
                    )
                },
            };

            return (options, data);
        }

        public async Task<int> UpdateInheritedForUuid(Guid uuid, QueryOptions? options = null)
        {
            (options, DataDictionary data) = GetOptionsForUpdateInherited(options);
            options.AddFilter("Uuid", uuid);

            return await base.UpdateAsync(data, options);
        }

        public async Task<int> UpdateInheritedForStoreUuid(Guid storeUuid, QueryOptions? options = null)
        {
            (options, DataDictionary data) = GetOptionsForUpdateInherited(options);
            options.AddFilter("store.Uuid", storeUuid);
            data["Location"] = Op.Column("store.Location");

            return await UpdateAsync(data, options);
        }

        public async Task<int> UpdateInheritedForCommerceUuid(Guid commerceUuid, QueryOptions? options = null)
        {
            (options, DataDictionary data) = GetOptionsForUpdateInherited(options);
            options.AddFilter("commerce.Uuid", commerceUuid);

            return await UpdateAsync(data, options);
        }
    }
}