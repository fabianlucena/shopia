using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IRepositories;
using backend_shopia.IServices;
using backend_shopia.QueryOptions;
using RFServices.Services;

namespace backend_shopia.Services;

public class PlanService(
    IPlanRepository planRepository,
    IServiceProvider serviceProvider
)
    : ANominableEntityService<Plan>(planRepository, serviceProvider),
     IPlanService
{
    public override async Task<Plan> ValidateForCreateAsync(Plan data)
    {
        data = await base.ValidateForCreateAsync(data);

        if (string.IsNullOrWhiteSpace(data.Name))
            throw new NoNameException();

        var existent = await GetSingleOrDefaultByNameAsync(data.Name);
        if (existent != null)
            throw new PlanAlreadyExistsException();

        return data;
    }

    public async Task<Plan> GetBaseAsync()
        => await GetSingleByNameAsync("Base");

    public async Task<Plan> GetSingleOrBaseAsync(PlanQueryOptions options)
        => await GetSingleOrDefaultAsync(options)
            ?? await GetBaseAsync();

    public async Task<PlanLimits> GetLimitsByPlanAsync(Plan plan, PlanQueryOptions? options = null)
    {
        var planLimitService = ServiceProvider.GetRequiredService<IPlanLimitService>();

        var extendedPlans = new List<long>();
        var limits = new List<PlanLimit>();
        var extendedPlan = plan;
        while (extendedPlan != null)
        {
            if (extendedPlan.IsActive)
            {
                var planLimitOptions = new PlanLimitQueryOptions
                {
                    PlanId = extendedPlan.Id
                };
                var extendLimits = await planLimitService.GetListAsync(planLimitOptions);
                foreach (var limit in extendLimits)
                {
                    var current = limits.Find(l => l.Name == limit.Name);
                    if (current == null)
                        limits.Add(limit);
                    else if (limit.Limit > current.Limit)
                        current.Limit = limit.Limit;
                }
            }

            if (extendedPlan.IncludesId == null)
                break;

            extendedPlans.Add(extendedPlan.Id);

            var extendsToOptions = new PlanQueryOptions
            {
                IncludeInactive = true,
                Id = extendedPlan.IncludesId.Value,
            };
            extendedPlan = await GetSingleOrDefaultAsync(extendsToOptions);
            if (extendedPlan != null && extendedPlans.Contains(extendedPlan.Id))
                break;
        }

        var basePlan = await GetSingleOrDefaultByNameAsync("Base");
        if (basePlan is not null)
        {
            var planLimitOptions = new PlanLimitQueryOptions
            {
                PlanId = basePlan.Id,
                SkipNames = limits.Select(l => l.Name),
            };
            var includesLimits = await planLimitService.GetListAsync(planLimitOptions);
            if (includesLimits.Any())
                limits.AddRange(includesLimits);
        }

        var result = new PlanLimits(limits);
        return result;
    }

    public async Task<MyPlanResponse> GetMyPlanAsync()
    {
        var userPlanService = ServiceProvider.GetRequiredService<IUserPlanService>();
        
        var plan = await userPlanService.GetSinglePlanByCurrentUserAsync();
        var limits = await GetLimitsByPlanAsync(plan);
        var used = await userPlanService.GetUsedPlanByCurrentUserAsync();
        return new MyPlanResponse
        {
            Uuid = plan.Uuid,
            Name = plan.Name,
            Description = plan.Description,
            Price = plan.Price,
            Limits = limits.ToDictionaryLCFirst(),
            Used = used,
        };
    }
}
