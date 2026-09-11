using RFIServices.QueryOptions;

namespace backend_shopia.QueryOptions;

public class UserPlanQueryOptions : CommonJoinQueryOptions
{
    public bool JoinPlan { get; set; } = false;

    public long? PlanId { get; set; }
    public IEnumerable<string>? SkipNames { get; set; }
    public string? UserId { get; set; }
    public DateTime? ValidUntil { get; set; }

    public bool OrderByExpirationDateDesc { get; set; }

    public UserPlanQueryOptions() { }

    public UserPlanQueryOptions(UserPlanQueryOptions? options)
        : base(options)
    {
        if (options is null)
            return;

        JoinPlan = options.JoinPlan;

        PlanId = options.PlanId;
        SkipNames = options.SkipNames;
        UserId = options.UserId;
        ValidUntil = options.ValidUntil;
        OrderByExpirationDateDesc = options.OrderByExpirationDateDesc;
    }

    public override UserPlanQueryOptions Clone()
        => new(this);

    public override UserPlanQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        return this;
    }
}
