using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IRepositories;
using backend_shopia.IServices;
using backend_shopia.QueryOptions;
using RFBase.ILibs;
using RFIServices.QueryOptions;
using RFServices.Services;

namespace backend_shopia.Services;

public class StoreService(
    IStoreRepository storeRepository,
    IServiceProvider serviceProvider
)
    : ANominableEntityService<Store>(storeRepository, serviceProvider),
    IStoreService
{
    public override async Task<Store> ValidateForCreateAsync(Store data)
    {
        data = await base.ValidateForCreateAsync(data);

        if (string.IsNullOrWhiteSpace(data.Name))
            throw new NoNameException();

        if (data.CommerceId <= 0)
        {
            data.CommerceId = data.Commerce?.Id ?? 0;
            if (data.CommerceId <= 0)
                throw new NoCommerceException();
        }

        var commerceService = ServiceProvider.GetRequiredService<ICommerceService>();
        _ = await commerceService.GetSingleOrDefaultByIdAsync(data.CommerceId)
            ?? throw new CommerceDoesNotExistException();

        var existent = await GetSingleOrDefaultAsync(new StoreQueryOptions
        {
            CommerceId = data.CommerceId,
            Name = data.Name,
        });

        if (existent != null)
            throw new AStoreForThatNameAlreadyExistException();

        var userPlanService = ServiceProvider.GetRequiredService<IUserPlanService>();
        var limits = await userPlanService.GetLimitsByCurrentUserAsync();

        var totalStoresCount = await GetCountByCurrentUserAsync(new StoreQueryOptions { IncludeInactive = true });
        if (totalStoresCount >= limits[PlanLimitName.MaxTotalStores])
            throw new TotalStoresLimitReachedException();

        var activeStoresCount = await GetCountByCurrentUserAsync();
        var activeStoresMax = limits[PlanLimitName.MaxEnabledStores];
        if (data.IsActive && activeStoresCount >= activeStoresMax
            || activeStoresCount > activeStoresMax
        )
        {
            throw new MaxActiveStoresLimitReachedException();
        }

        return data;
    }

    public override async Task<IDataDictionary> ValidateForUpdateAsync(IDataDictionary data, BaseQueryOptions options)
    {
        data = await base.ValidateForUpdateAsync(data, options);

        if (data.GetBool("IsActive"))
        {
            var storeOptions = options.Clone() as StoreQueryOptions
                ?? throw new InvalidOperationException("Options must be of type StoreQueryOptions.");

            storeOptions.IncludeInactive = true;
            storeOptions.CommerceOwnerId = await GetCurrentUserIdAsync();
            var current = await GetSingleOrDefaultAsync(storeOptions)
                ?? throw new StoreDoesNotExistException();

            if (!current.IsActive)
            {
                var userPlanService = ServiceProvider.GetRequiredService<IUserPlanService>();
                var limits = await userPlanService.GetLimitsByCurrentUserAsync();

                var activeStoresCount = await GetCountByCurrentUserAsync();
                var activeStoresMax = limits[PlanLimitName.MaxEnabledStores];
                if (activeStoresCount >= activeStoresMax)
                    throw new MaxActiveStoresLimitReachedException();
            }
        }

        return data;
    }

    public async Task<bool> CheckByUuidAndCurrentUserAsync(Guid uuid, StoreQueryOptions? options = null)
    {
        options = options?.Clone() ?? new();
        options.IncludeCommerce = true;
        options.IncludeInactive = true;
        options.Uuid = uuid;
        options.CommerceOwnerId = await GetCurrentUserIdAsync();
        _ = await GetSingleOrDefaultAsync(options)
            ?? throw new StoreDoesNotExistException();

        return true;
    }

    public async Task<StoreQueryOptions> GetFilterByOwnerIdAsync(long ownerId, StoreQueryOptions? options = null)
    {
        var commerceService = ServiceProvider.GetRequiredService<ICommerceService>();
        var commercesId = await commerceService.GetListIdByOwnerIdAsync(ownerId, new CommerceQueryOptions { IncludeInactive = options?.IncludeInactive ?? false });

        options = options?.Clone() ?? new();
        options.CommercesId = commercesId;

        return options;
    }

    public async Task<int> GetCountByOwnerIdAsync(long ownerId, StoreQueryOptions? options = null)
        => await GetCountAsync(await GetFilterByOwnerIdAsync(ownerId, options));
    
    public async Task<int> GetCountByCurrentUserAsync(StoreQueryOptions? options = null)
        => await GetCountByOwnerIdAsync(await GetCurrentUserIdAsync(), options);

    public async Task<IEnumerable<long>> GetListIdByOwnerIdAsync(long ownerId, StoreQueryOptions? options = null)
        => await GetListIdAsync(await GetFilterByOwnerIdAsync(ownerId, options));

    public async Task<IEnumerable<long>> GetListIdByCurrentUserAsync(StoreQueryOptions? options = null)
        => await GetListIdByOwnerIdAsync(await GetCurrentUserIdAsync(), options);
}

