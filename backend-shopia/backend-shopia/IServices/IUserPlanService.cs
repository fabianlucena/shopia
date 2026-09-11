using backend_shopia.DTO;
using backend_shopia.Entities;
using RFIServices.IServices;

namespace backend_shopia.IServices;

public interface IUserPlanService
    : ICommonJoinService<UserPlan>, IGetCurrentAndSystemUserService
{
    Task<Plan> GetSinglePlanByCurrentUserAsync();

    Task<UsedPlanDTO> GetUsedPlanByCurrentUserAsync();

    Task<PlanLimits> GetLimitsByCurrentUserAsync();
}
