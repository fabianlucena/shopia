using backend_shopia.Entities;
using RFService.IServices;

namespace backend_shopia.IServices
{
    public interface IItemPriceLogService
        : IService<ItemPriceLog>,
            IServiceCreatedAt<ItemPriceLog>
    {
    }
}
