using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.QueryOptions;
using RFIServices.IServices;

namespace backend_shopia.IServices;

public interface IPlanService
    : IANominableEntityService<Plan>
{
    Task<Plan> GetBaseAsync();

    Task<Plan> GetSingleOrBaseAsync(PlanQueryOptions options);

    Task<PlanLimits> GetLimitsForPlanAsync(Plan plan, PlanQueryOptions? options = null);

    Task<MyPlanResponse> GetMyPlanAsync();
}
