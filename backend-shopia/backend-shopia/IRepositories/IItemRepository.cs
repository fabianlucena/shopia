using backend_shopia.Entities;
using backend_shopia.QueryOptions;
using RFIRepositories.IRepositories;

namespace backend_shopia.IRepositories;

public interface IItemRepository : IANominableEntityRepository<Item>
{
    Task<int> UpdateInheritedByUuidAsync(Guid uuid);
    Task<int> UpdateInheritedByStoreUuidAsync(Guid storeUuid);
    Task<int> UpdateInheritedByCommerceUuidAsync(Guid commerceUuid);
}
