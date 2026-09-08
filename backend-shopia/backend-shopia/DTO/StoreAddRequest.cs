using backend_shopia.Entities;
using backend_shopia.IServices;
using backend_shopia.Types;

namespace backend_shopia.DTO;

public class StoreAddRequest
{
    public required Guid CommerceUuid { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public LatLng? Location { get; set; }

    public async Task<Store> ToStoreAsync(IServiceProvider serviceProvider)
    {
        var commerceService = serviceProvider.GetRequiredService<ICommerceService>();

        return new Store
        {
            CommerceId = await commerceService.GetSingleIdByUuidAsync(CommerceUuid),
            Name = Name,
            Description = Description,
            Location = Location?.ToGeometryPoint(),
        };
    }
}
