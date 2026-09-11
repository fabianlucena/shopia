using RFIServices.QueryOptions;

namespace backend_shopia.QueryOptions;

public class PlanLimitQueryOptions : ANominableEntityQueryOptions
{
    public long? PlanId { get; set; }
    public IEnumerable<string>? SkipNames { get; set; }

    public PlanLimitQueryOptions() { }

    public PlanLimitQueryOptions(PlanLimitQueryOptions? options)
        : base(options)
    {
        if (options is null)
            return;

        PlanId = options.PlanId;
        SkipNames = options.SkipNames;
    }

    public override PlanLimitQueryOptions Clone()
        => new(this);

    public override PlanLimitQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        return this;
    }
}
