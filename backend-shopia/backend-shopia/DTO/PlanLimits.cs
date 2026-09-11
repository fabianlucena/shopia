using backend_shopia.Entities;
using backend_shopia.Exceptions;

namespace backend_shopia.DTO;

public class PlanLimits
{
    private List<PlanLimit> Limits { get; } = [];

    public long this[PlanLimitName name]
    {
        get => Limits.Find(l => l.Name == name.ToString())?.Limit
            ?? throw new NoLimitNameInPlanException(name.ToString());
    }

    public PlanLimits(IEnumerable<PlanLimit> limits)
    {
        Limits.AddRange(limits);
    }

    public Dictionary<string, long> ToDictionary()
        => Limits.ToDictionary(l => l.Name, l => l.Limit);

    public Dictionary<string, long> ToDictionaryLCFirst()
        => Limits.ToDictionary(l => char.ToLowerInvariant(l.Name[0]) + l.Name[1..],l => l.Limit);
}
