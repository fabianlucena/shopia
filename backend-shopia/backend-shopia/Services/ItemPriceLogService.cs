using backend_shopia.Entities;
using backend_shopia.IRepositories;
using backend_shopia.IServices;
using RFServices.Services;

namespace backend_shopia.Services;

public class ItemPriceLogService(
    IItemPriceLogRepository itemPriceLogRepository,
    IServiceProvider serviceProvider
)
    : CreatableEntityService<ItemPriceLog>(itemPriceLogRepository, serviceProvider),
        IItemPriceLogService
{
}