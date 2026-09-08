using backend_shopia.Entities;

namespace backend_shopia.DTO;

public class CommerceAddRequest
{
    public bool? IsActive { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    public Commerce ToCommerce()
    {
        return new Commerce
        {
            IsActive = IsActive ?? true,
            Name = Name,
            Description = Description
        };
    }
}
