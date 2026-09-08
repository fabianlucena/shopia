using backend_shopia.Entities;

namespace backend_shopia.DTO;

public class PlanDTO(Plan plan)
{
    public Guid? Uuid { get; set; } = plan.Uuid;
    public string? Name { get; set; } = plan.Name;
    public string? Description { get; set; } = plan.Description;
}