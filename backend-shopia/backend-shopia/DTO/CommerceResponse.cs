using backend_shopia.Entities;

namespace backend_shopia.DTO;

public class CommerceResponse(Commerce commerce)
    : CommerceMinimalDTO(commerce)
{
    public bool IsActive { get; set; } = commerce.IsActive;
    public PlanDTO? Plan { get; set; } = commerce.Plan is not null ? new PlanDTO(commerce.Plan) : null;
    public IEnumerable<StoreMinimalDTO>? Stores { get; set; } = commerce.Stores?.Select(store => new StoreMinimalDTO(store));
    public DateTime CreatedAt { get; set; } = commerce.CreatedAt;
    public DateTime UpdatedAt { get; set; } = commerce.UpdatedAt;
    public DateTime? DeletedAt { get; set; } = commerce.DeletedAt;
}
