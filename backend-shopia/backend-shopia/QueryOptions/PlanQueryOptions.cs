using RFIServices.QueryOptions;

namespace backend_shopia.QueryOptions;

public class PlanQueryOptions : ANominableEntityQueryOptions
{
    public PlanQueryOptions() { }

    public PlanQueryOptions(PlanQueryOptions? options)
        : base(options)
    {
        if (options is null)
            return;
    }

    public override PlanQueryOptions Clone()
        => new(this);

    public override PlanQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        return this;
    }
}
