using RFEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shopia.Entities;

[Table("Plans", Schema = "shopia")]
public class Plan
    : ANominableEntity
{
    [ForeignKey("Includes")]
    public long? IncludesId { get; set; } = default;
    public Plan? Includes { get; set; } = default;

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

        IncludesId = plan.IncludesId;
        Includes = plan.Includes;
        Description = plan.Description;
        Price = plan.Price;
    }

    public override Plan Clone()
        => new(this);
}