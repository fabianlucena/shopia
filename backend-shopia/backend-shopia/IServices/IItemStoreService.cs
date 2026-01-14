using backend_shopia.DTO;
using backend_shopia.Entities;
using RFService.IServices;
using RFService.Repo;

namespace backend_shopia.IServices
{
    public interface IItemStoreService
        : IService<ItemStore>,
            IServiceCreatedAt<ItemStore>
    {
        Task<IEnumerable<ItemStore>> GetListForItemIdAsync(Int64 itemId, QueryOptions? options = null);

        Task<IEnumerable<Store>> GetListStoresForItemIdAsync(Int64 itemId, QueryOptions? options = null);
    }
}
