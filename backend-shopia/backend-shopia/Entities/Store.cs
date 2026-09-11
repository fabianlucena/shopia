using NetTopologySuite.Geometries;
using RFEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shopia.Entities;

[Table("Stores", Schema = "shopia")]
public class Store
    : ANominableEntity
{
    [Required]
    [ForeignKey("Commerce")]
    public long CommerceId { get; set; } = default;
    public Commerce? Commerce { get; set; } = default;

    public string? Description { get; set; }

    public Point? Location { get; set; }

    public Store() { }

    public Store(Store store)
        : base(store)
    {
        CommerceId = store.CommerceId;
        Commerce = store.Commerce;
        Description = store.Description;
        Location = store.Location;
    }

    public override Store Clone()
        => new(this);
}