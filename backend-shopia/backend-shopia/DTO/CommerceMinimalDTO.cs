using backend_shopia.Entities;

namespace backend_shopia.DTO;

public class CommerceMinimalDTO(Commerce commerce)
{
    public Guid Uuid { get; set; } = commerce.Uuid;
    public string Name { get; set; } = commerce.Name;
    public string Description { get; set; } = commerce.Description;
}