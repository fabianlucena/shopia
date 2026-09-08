using backend_shopia.Entities;

namespace backend_shopia.DTO;

public class CategoryResponse(Category category)
{
    public Guid Uuid { get; set; } = category.Uuid;
    public bool IsActive { get; set; } = category.IsActive;
    public string Name { get; set; } = category.Name;
    public string Description { get; set; } = category.Description;
    public DateTime CreatedAt { get; set; } = category.CreatedAt;
    public DateTime UpdatedAt { get; set; } = category.UpdatedAt;
    public DateTime? DeletedAt { get; set; } = category.DeletedAt;
}
