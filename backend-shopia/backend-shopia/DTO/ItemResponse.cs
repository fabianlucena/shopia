using backend_shopia.Entities;
using System.Globalization;

namespace backend_shopia.DTO;

public class ItemResponse(Item item)
{
    public Guid Uuid { get; set; } = item.Uuid;
    public bool IsActive { get; set; } = item.IsActive;
    public string Name { get; set; } = item.Name;
    public string Description { get; set; } = item.Description;
    public Guid? CommerceUuid { get; set; } = item.Commerce?.Uuid;
    public CommerceMinimalDTO? Commerce { get; set; } = item.Commerce is not null ? new CommerceMinimalDTO(item.Commerce) : null;
    public CategoryMinimalDTO? Category { get; set; } = item.Category is not null ? new CategoryMinimalDTO(item.Category) : null;
    public Guid? CategoryUuid { get; set; } = item.Category?.Uuid;
    public IEnumerable<StoreMinimalDTO>? Stores { get; set; } = item.Stores?.Select(s => new StoreMinimalDTO(s));
    public IEnumerable<Guid>? StoresUuid { get; set; } = item.Stores?.Select(s => s.Uuid);
    public string Price { get; set; } = item.Price.ToString(CultureInfo.InvariantCulture);
    public int? Stock { get; set; } = item.Stock;
    public bool IsPresent { get; set; } = item.IsPresent;
    public int? MinAge { get; set; } = item.MinAge;
    public int? MaxAge { get; set; } = item.MaxAge;
    public bool IsMine { get; set; }
    public DateTime CreatedAt { get; set; } = item.CreatedAt;
    public DateTime UpdatedAt { get; set; } = item.UpdatedAt;
    public DateTime? DeletedAt { get; set; } = item.DeletedAt;
    public IEnumerable<ItemImageDTO> Images { get; set; } = [];
}
