using backend_shopia.Entities;
using backend_shopia.IServices;
using RFService.IRepo;
using RFService.Services;

namespace backend_shopia.Services
{
    public class ItemPriceLogService(
        IRepo<ItemPriceLog> repo
    )
        : ServiceCreatedAt<IRepo<ItemPriceLog>, ItemPriceLog>(repo),
            IItemPriceLogService
    {
    }
}