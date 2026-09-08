using backend_shopia.Entities;

namespace backend_shopia.DTO;

public class CategoryResponse(Category category)
    : CategoryMinimalDTO(category)
{
    public bool IsActive { get; set; } = category.IsActive;
    public DateTime CreatedAt { get; set; } = category.CreatedAt;
    public DateTime UpdatedAt { get; set; } = category.UpdatedAt;
    public DateTime? DeletedAt { get; set; } = category.DeletedAt;
}
