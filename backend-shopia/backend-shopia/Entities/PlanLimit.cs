using RFEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_shopia.Entities;

[Table("PlansLimits", Schema = "shopia")]
public class PlanLimit
    : ANominableEntity
{
    [Required]
    [ForeignKey("Plan")]
    public Int64 PlanId { get; set; } = default;
    public Plan? Plan { get; set; } = default;

    public string? Description { get; set; }

    public Int64 Limit { get; set; }

    public PlanLimit() { }

    public PlanLimit(PlanLimit? planLimit)
        : base(planLimit)
    {
        if (planLimit is null)
            return;

        PlanId = planLimit.PlanId;
        Plan = planLimit.Plan;
        Description = planLimit.Description;
        Limit = planLimit.Limit;
    }

    public override PlanLimit Clone()
        => new(this);
}