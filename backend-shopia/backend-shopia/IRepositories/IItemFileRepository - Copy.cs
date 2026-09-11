using backend_shopia.Entities;
using backend_shopia.QueryOptions;
using RFIRepositories.IRepositories;

namespace backend_shopia.IRepositories;

public interface IItemFileRepository : ICreatableWithNameEntityRepository<ItemFile>
{
    Task<long> GetAggregatedSizeAsync(ItemFileQueryOptions? options = null);
    /*{
        options = await GetFilterByOwnerIdAsync(ownerId, options);
        options.Select ??= [Op.Sum(Op.DataLength("Content"))];

        return await GetLongAsync(options) ?? 0;
    }*/
}