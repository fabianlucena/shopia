using backend_shopia.Entities;
using backend_shopia.Types;

namespace backend_shopia.DTO;

public class StoreResponse(Store store)
    : StoreMinimalDTO(store)
{
    public bool IsActive { get; set; } = store.IsActive;
    public CommerceMinimalDTO? Commerce { get; set; } = store.Commerce is not null ? new CommerceMinimalDTO(store.Commerce) : null;
    public DateTime CreatedAt { get; set; } = store.CreatedAt;
    public DateTime UpdatedAt { get; set; } = store.UpdatedAt;
    public DateTime? DeletedAt { get; set; } = store.DeletedAt;
    public LatLng? Location { get; set; } = store.Location is not null ? new LatLng(store.Location) : null;
}