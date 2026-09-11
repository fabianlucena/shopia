using RFEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shopia.Entities;

[Table("Plans", Schema = "shopia")]
public class Plan
    : ANominableEntity
{
    [ForeignKey("ExtendTo")]
    public long? ExtendToId { get; set; } = default;
    public Plan? ExtendTo { get; set; } = default;

    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public Plan() { }

    public Plan(Plan? plan)
        : base(plan)
    {
        if (plan is null)
            return;

        ExtendToId = plan.ExtendToId;
        ExtendTo = plan.ExtendTo;
        Description = plan.Description;
        Price = plan.Price;
    }

    public override Plan Clone()
        => new(this);
}