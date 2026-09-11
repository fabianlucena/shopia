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

public class CommerceService(
    ICommerceRepository commerceRepository,
    IServiceProvider serviceProvider
)
    : ANominableOwnedEntityService<Commerce>(commerceRepository, serviceProvider),
    ICommerceService
{
    public override async Task<Commerce> ValidateForCreateAsync(Commerce data)
    {
        data = await base.ValidateForCreateAsync(data);

        if (string.IsNullOrWhiteSpace(data.Name))
            throw new NoNameException();

        if (data.OwnerId <= 0)
        {
            data.OwnerId = data.Owner?.Id ?? 0;
            if (data.OwnerId <= 0)
                throw new NoOwnerException();
        }

        var existent = await GetFirstOrDefaultAsync(new CommerceQueryOptions
        {
            OwnerId = data.OwnerId,
            Name = data.Name,
        });

        if (existent != null)
            throw new ACommerceForThatNameAlreadyExistException();

        var userPlanService = ServiceProvider.GetRequiredService<IUserPlanService>();
        var limits = await userPlanService.GetLimitsByCurrentUserAsync();

        var totalCommercesCount = await GetCountByCurrentUserAsync(new CommerceQueryOptions { IncludeInactive = true });
        if (totalCommercesCount >= limits[PlanLimitName.MaxTotalCommerces])
            throw new TotalCommercesLimitReachedException();

        var enabledCommercesCount = await GetCountByCurrentUserAsync();
        var enabledCommercesMax = limits[PlanLimitName.MaxEnabledCommerces];
        if (data.IsActive && enabledCommercesCount >= enabledCommercesMax
            || enabledCommercesCount > enabledCommercesMax
        )
        {
            throw new MaxEnabledCommercesLimitReachedException();
        }

        return data;
    }

    public override async Task<IEnumerable<Commerce>> GetListAsync(BaseQueryOptions options)
    {
        var commerces = await base.GetListAsync(options);
        if (commerces.Any() && options is CommerceQueryOptions commerceOptions)
        {
            if (commerceOptions.IncludeStores == true)
            {
                var storeService = ServiceProvider.GetRequiredService<IStoreService>();
                foreach (var commerce in commerces)
                {
                    var stores = await storeService.GetListAsync(new StoreQueryOptions { CommerceId = commerce.Id } );
                    if (!stores.Any())
                        continue;

                    commerce.Stores = stores;
                }
            }
        }

        return commerces;
    }

    public override async Task<IDataDictionary> ValidateForUpdateAsync(IDataDictionary data, BaseQueryOptions options)
    {
        data = await base.ValidateForUpdateAsync(data, options);

        if (data.GetBool("IsActive"))
        {
            var getOptions = new CommerceQueryOptions { IncludeInactive = true };
            _ = await GetSingleOrDefaultAsync(getOptions)
                ?? throw new CommerceDoesNotExistException();

            var userPlanService = ServiceProvider.GetRequiredService<IUserPlanService>();
            var limits = await userPlanService.GetLimitsByCurrentUserAsync();

            var enabledCommercesCount = await GetCountByCurrentUserAsync();
            var enabledCommercesMax = limits[PlanLimitName.MaxEnabledCommerces];
            if (enabledCommercesCount >= enabledCommercesMax)
                throw new MaxEnabledCommercesLimitReachedException();
        }

        return data;
    }

    /*public async Task<bool> CheckByUuidAndCurrentUserAsync(Guid uuid, CommerceQueryOptions? options = null)
    {
        var ownerId = await GetCurrentUserIdAsync();

        options = options?.Clone() ?? new CommerceQueryOptions();
        // No se debe usar HttpContext.Request en esta capa sino en la de Controllers
        options.UpdateFromRequest(HttpContext.Request);
        options.IncludeInactive = true;
        options.OwnerId = ownerId;
        options.Uuid = uuid;

        if (await GetSingleOrDefaultAsync(options) != null)
            return true;

        throw new CommerceDoesNotExistException();
    }*/

    public CommerceQueryOptions GetFilterByOwnerIdAsync(long ownerId, CommerceQueryOptions? options = null)
    {
        options = options?.Clone() ?? new CommerceQueryOptions();
        options.OwnerId = ownerId;

        return options;
    }

    public async Task<int> GetCountByOwnerIdAsync(long ownerId, CommerceQueryOptions? options = null)
        => await GetCountAsync(GetFilterByOwnerIdAsync(ownerId, options));

    public async Task<int> GetCountByCurrentUserAsync(CommerceQueryOptions? options = null)
        => await GetCountByOwnerIdAsync(await GetCurrentUserIdAsync(), options);

    public async Task<IEnumerable<long>> GetListIdByOwnerIdAsync(long ownerId, CommerceQueryOptions? options = null)
        => await GetListIdAsync(GetFilterByOwnerIdAsync(ownerId, options));

    public async Task<IEnumerable<Guid>> GetListUuidByOwnerIdAsync(long ownerId, CommerceQueryOptions? options = null)
        => await GetListUuidAsync(GetFilterByOwnerIdAsync(ownerId, options));

    public async Task<IEnumerable<long>> GetListIdByCurrentUserAsync(CommerceQueryOptions? options = null)
        => await GetListIdByOwnerIdAsync(await GetCurrentUserIdAsync(), options);

    public async Task<IEnumerable<Guid>> GetListUuidByCurrentUserAsync(CommerceQueryOptions? options = null)
        => await GetListUuidByOwnerIdAsync(await GetCurrentUserIdAsync(), options);
}

