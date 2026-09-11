using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IRepositories;
using backend_shopia.IServices;
using backend_shopia.QueryOptions;
using RFServices.Services;

namespace backend_shopia.Services;

public class ItemFileService(
    IItemFileRepository itemFileRepository,
    IServiceProvider serviceProvider
)
    : CreatableWithNameEntityService<ItemFile>(itemFileRepository, serviceProvider),
        IItemFileService
{
    public override async Task<ItemFile> ValidateForCreateAsync(ItemFile data)
    {
        data = await base.ValidateForCreateAsync(data);

        if (string.IsNullOrWhiteSpace(data.Name))
            throw new NoNameException();

        if (data.Item == null)
        {
            if (data.ItemId <= 0)
                throw new NoItemException();

            var itemService = ServiceProvider.GetRequiredService<IItemService>();
            data.Item = await itemService.GetSingleOrDefaultByIdAsync(
                data.ItemId,
                new ItemFileQueryOptions
                {
                    JoinCommerce = true,
                }
            )
                ?? throw new NoItemException();
        }

        var item = data.Item;
        if (item.Commerce == null)
        {
            if (item.Stores == null || !item.Stores.Any())
                throw new NoStoreProvidedException();

            var store = item.Stores.First()
                ?? throw new NoStoreProvidedException();

            if (store.Commerce == null)
            {
                if (store.Id <= 0)
                    throw new NoCommerceException();

                var storeService = ServiceProvider.GetRequiredService<IStoreService>();
                store = await storeService.GetSingleOrDefaultByIdAsync(
                    store.Id,
                    new ItemFileQueryOptions
                    {
                        JoinCommerce = true,
                        IncludeInactive = true,
                    }
                );

                if (store == null)
                    throw new NoStoreProvidedException();

                if (store.Commerce == null)
                    throw new NoCommerceException();
            }

            if (store.Commerce == null)
                throw new CommerceDoesNotExistException();
        }

        var userPlanService = ServiceProvider.GetRequiredService<IUserPlanService>();
        var limits = await userPlanService.GetLimitsByCurrentUserAsync();

        if (data.Content.Length > limits[PlanLimitName.MaxItemImageSize])
            throw new ImageIsTooLargeException(data.Content.Length, limits[PlanLimitName.MaxItemImageSize]);

        var itemImagesCount = await GetCountAsync(new ItemFileQueryOptions
        {
            IncludeInactive = true,
            ItemId = data.ItemId,
        });
        if (itemImagesCount >= limits[PlanLimitName.MaxTotalImagesPerSingleItem])
            throw new TotalImagesPerItemLimitReachedException();

        var totalCount = await GetCountByCurrentUserAsync(new ItemFileQueryOptions { IncludeInactive = true });
        if (totalCount >= limits[PlanLimitName.MaxTotalItemsImages])
            throw new TotalItemsImagesLimitReachedException();

        var enabledCount = await GetCountByCurrentUserAsync();
        if (enabledCount > limits[PlanLimitName.MaxEnabledItemsImages])
            throw new MaxEnabledItemsImagesLimitReachedException();

        var aggregatedSize = await GetAggregatedSizeByCurrentUserAsync(new ItemFileQueryOptions { IncludeInactive = true });
        aggregatedSize += data.Content.Length;
        if (aggregatedSize >= limits[PlanLimitName.MaxItemsImagesAggregatedSize])
            throw new TotalItemsImagesAggregatedSizeLimitReachedException();

        var enabledAggregatedSize = await GetAggregatedSizeByCurrentUserAsync();
        enabledAggregatedSize += data.Content.Length;
        if (enabledAggregatedSize >= limits[PlanLimitName.MaxEnabledItemsImagesAggregatedSize])
            throw new MaxEnabledItemsImagesAggregatedSizeLimitReachedException();

        return data;
    }

    public async Task<ItemFileQueryOptions> GetFilterByOwnerIdAsync(long ownerId, ItemFileQueryOptions? options = null)
    {
        var itemService = ServiceProvider.GetRequiredService<IItemService>();
        var itemsId = await itemService.GetListIdByOwnerIdAsync(ownerId, new ItemQueryOptions {
            IncludeInactive = options?.IncludeInactive ?? false,
            Id = options?.ItemId,
            Ids = options?.ItemsId,
        });

        options = options?.Clone() ?? new ItemFileQueryOptions();
        options.ItemsId = itemsId;

        return options;
    }

    public async Task<int> GetCountByOwnerIdAsync(long ownerId, ItemFileQueryOptions? options = null)
        => await GetCountAsync(await GetFilterByOwnerIdAsync(ownerId, options));

    public async Task<int> GetCountByCurrentUserAsync(ItemFileQueryOptions? options = null)
        => await GetCountByOwnerIdAsync(await GetCurrentUserIdAsync(), options);

    public async Task<long> GetAggregatedSizeByOwnerIdAsync(long ownerId, ItemFileQueryOptions? options = null)
    {
        options = await GetFilterByOwnerIdAsync(ownerId, options);
        return await itemFileRepository.GetAggregatedSizeAsync(options);
    }

    public async Task<long> GetAggregatedSizeByCurrentUserAsync(ItemFileQueryOptions? options = null)
        => await GetAggregatedSizeByOwnerIdAsync(await GetCurrentUserIdAsync(), options);

    public async Task<IEnumerable<ItemFile>> AddByItemUuidAsync(Guid itemUuid, FilesCollectionDTO files)
    {
        var itemService = ServiceProvider.GetRequiredService<IItemService>();
        return await AddByItemIdAsync(await itemService.GetSingleIdByUuidAsync(itemUuid), files);
    }

    public async Task<IEnumerable<ItemFile>> AddByItemIdAsync(long itemId, FilesCollectionDTO files)
    {
        var result = new List<ItemFile>();
        foreach (var file in files)
        {
            if (file.Content.Length == 0)
                continue;

            var itemImage = new ItemFile
            {
                ItemId = itemId,
                Name = file.Name,
                ContentType = file.ContentType,
                Content = file.Content,
            };

            result.Add(await CreateAsync(itemImage));
        }

        return result;
    }

    public async Task<IEnumerable<ItemFile>> GetListByItemIdAsync(long itemId)
        => await GetListAsync(new ItemFileQueryOptions { ItemId = itemId });
}