using backend_shopia.Entities;
using RFIRepositories.IRepositories;

namespace backend_shopia.IRepositories;

public interface IItemStoreRepository : ICreatableJoinRepository<ItemStore>
{
}