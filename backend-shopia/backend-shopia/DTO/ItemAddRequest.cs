using backend_shopia.Entities;

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

    public Item ToItem()
    {
        return new Item
        {
            IsActive = IsActive,
            Name = Name,
            Description = Description,
            CommerceUuid = CommerceUuid,
            CategoryUuid = CategoryUuid,
            StoresUuid = StoresUuid,
            Price = Price,
            Stock = Stock,
            IsPresent = IsPresent,
            MinAge = MinAge,
            MaxAge = MaxAge
        };
    }
}
