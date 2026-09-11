using backend_shopia.Entities;
using backend_shopia.IServices;

namespace backend_shopia.DTO;

public class ItemAddRequest
{
    public bool IsActive { get; set; } = true;
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required Guid CommerceUuid { get; set; }
    public required Guid CategoryUuid { get; set; }
    public required Guid[] StoresUuid { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsPresent { get; set; } = false;
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    
    public async Task<Item> ToItemAsync(IServiceProvider serviceProvider)
    {
        var commerceService = serviceProvider.GetRequiredService<ICommerceService>();
        var categoryService = serviceProvider.GetRequiredService<ICategoryService>();
        var storeService = serviceProvider.GetRequiredService<IStoreService>();

        return new Item
        {
            IsActive = IsActive,
            Name = Name,
            Description = Description,
            CommerceId = await commerceService.GetSingleIdByUuidAsync(CommerceUuid),
            CategoryId = await categoryService.GetSingleIdByUuidAsync(CategoryUuid),
            Stores = await storeService.GetListByUuidsAsync(StoresUuid),
            Price = Price,
            Stock = Stock,
            IsPresent = IsPresent,
            MinAge = MinAge,
            MaxAge = MaxAge
        };
    }
}
