using backend_shopia.Entities;

namespace backend_shopia.DTO;

public class StoreMinimalDTO(Store store)
{
    public Guid Uuid { get; set; } = store.Uuid;
    public string Name { get; set; } = store.Name;
    public string? Description { get; set; } = store.Description;
}
