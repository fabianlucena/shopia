using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.IRepositories;
using backend_shopia.IServices;
using backend_shopia.QueryOptions;
using RFIServices.IServices;
using RFServices.Services;

namespace backend_shopia.Services;

public class UserPlanService(
    IUserPlanRepository userPlanRepository,
    IServiceProvider serviceProvider
)
    : CommonJoinService<UserPlan>(userPlanRepository, serviceProvider),
    IUserPlanService, IGetCurrentAndSystemUserService
{
    public async Task<long> GetCurrentUserIdAsync()
        => await GetRequiredService<IUserService>().GetCurrentUserIdAsync();

    public async Task<Plan> GetSinglePlanByCurrentUserAsync()
    {
        var ownerId = await GetCurrentUserIdAsync();

        var userPlan = await GetFirstOrDefaultAsync(
            new UserPlanQueryOptions
            {
                JoinPlan = true,
                UserId = ownerId,
                ValidUntil = DateTime.UtcNow,
                OrderByExpirationDateDesc = true,
                Take = 1,
            }
        );

        var planService = ServiceProvider.GetRequiredService<IPlanService>();

        var plan = userPlan?.Plan ?? await planService.GetBaseAsync();
        return plan;
    }

    public async Task<UsedPlanDTO> GetUsedPlanByCurrentUserAsync()
    {
        var ownerId = await GetCurrentUserIdAsync();

        var commerceService = ServiceProvider.GetRequiredService<ICommerceService>();
        var storeService = ServiceProvider.GetRequiredService<IStoreService>();
        var itemService = ServiceProvider.GetRequiredService<IItemService>();
        var itemFileService = ServiceProvider.GetRequiredService<IItemFileService>();
        var commerceFileService = ServiceProvider.GetRequiredService<ICommerceFileService>();

        var usedPlan = new UsedPlanDTO
        {
            TotalCommercesCount = await commerceService.GetCountByOwnerIdAsync(ownerId, new CommerceQueryOptions { IncludeInactive = true }),
            EnabledCommercesCount = await commerceService.GetCountByOwnerIdAsync(ownerId),
            TotalStoresCount = await storeService.GetCountByOwnerIdAsync(ownerId, new StoreQueryOptions { IncludeInactive = true }),
            EnabledStoresCount = await storeService.GetCountByOwnerIdAsync(ownerId),
            TotalItemsCount = await itemService.GetCountByOwnerIdAsync(ownerId, new ItemQueryOptions { IncludeInactive = true }),
            EnabledItemsCount = await itemService.GetCountByOwnerIdAsync(ownerId),
            TotalItemsImagesCount = await itemFileService.GetCountByOwnerIdAsync(ownerId, new ItemFileQueryOptions { IncludeInactive = true }),
            EnabledItemsImagesCount = await itemFileService.GetCountByOwnerIdAsync(ownerId),
            ItemsImagesAggregatedSize = await itemFileService.GetAggregatedSizeByOwnerIdAsync(ownerId, new ItemFileQueryOptions { IncludeInactive = true }),
            EnabledItemsImagesAggregatedSize = await itemFileService.GetAggregatedSizeByOwnerIdAsync(ownerId),
            TotalCommercesImagesCount = await commerceFileService.GetCountByOwnerIdAsync(ownerId, new CommerceFileQueryOptions { IncludeInactive = true }),
            EnabledCommercesImagesCount = await commerceFileService.GetCountByOwnerIdAsync(ownerId),
            CommercesImagesAggregatedSize = await commerceFileService.GetAggregatedSizeByOwnerIdAsync(ownerId, new CommerceFileQueryOptions { IncludeInactive = true }),
            EnabledCommercesImagesAggregatedSize = await commerceFileService.GetAggregatedSizeByOwnerIdAsync(ownerId),
        };

        return usedPlan;
    }

    public async Task<PlanLimits> GetLimitsByCurrentUserAsync()
    {
        var planService = ServiceProvider.GetRequiredService<IPlanService>();
        var plan = await GetSinglePlanByCurrentUserAsync();
        return await planService.GetLimitsByPlanAsync(plan);
    }
}
