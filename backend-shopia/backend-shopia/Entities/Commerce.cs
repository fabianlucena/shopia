using RFEntities.Attributes;
using RFEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shopia.Entities;

[Table("Commerces", Schema = "shopia")]
public class Commerce
    : ANominableOwnedEntity
{
    [Required]
    public string Description { get; set; } = string.Empty;

    [ForeignKey("Plan")]
    public long? PlanId { get; set; } = default;
    public Plan? Plan { get; set; } = default;

    [Virtual]
    public IEnumerable<Store>? Stores { get; set; } = default;

    public Commerce() { }

    public Commerce(Commerce commerce)
        : base(commerce)
    {
        if (commerce == null)
            return;

        Description = commerce.Description;
        PlanId = commerce.PlanId;
        Plan = commerce.Plan;
        Stores = commerce.Stores;
    }

    public override Commerce Clone()
        => new(this);
}