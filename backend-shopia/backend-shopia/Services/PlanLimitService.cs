using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IRepositories;
using backend_shopia.IServices;
using RFServices.Services;

namespace backend_shopia.Services;

public class PlanLimitService(
    IPlanLimitRepository planLimitRepository,
    IServiceProvider serviceProvider
)
    : ANominableEntityService<PlanLimit>(planLimitRepository, serviceProvider),
    IPlanLimitService
{
    public override async Task<PlanLimit> ValidateForCreateAsync(PlanLimit data)
    {
        data = await base.ValidateForCreateAsync(data);

        if (string.IsNullOrWhiteSpace(data.Name))
            throw new NoNameException();

        var existent = await GetSingleOrDefaultByNameAsync(data.Name);
        if (existent != null)
            throw new PlanAlreadyExistsException();

        return data;
    }
}
