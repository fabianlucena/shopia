using backend_shopia.Entities;

namespace backend_shopia.DTO;

public class CategoryMinimalDTO(Category category)
{
    public Guid Uuid { get; set; } = category.Uuid;
    public string Name { get; set; } = category.Name;
    public string Description { get; set; } = category.Description;
}
