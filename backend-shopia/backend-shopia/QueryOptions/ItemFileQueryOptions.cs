using RFIServices.QueryOptions;

namespace backend_shopia.QueryOptions;

public class ItemFileQueryOptions : NominableEntityQueryOptions
{
    public ItemFileQueryOptions() { }

    public ItemFileQueryOptions(ItemFileQueryOptions? options)
        : base(options)
    {
        if (options is null)
            return;
    }

    public override ItemFileQueryOptions Clone()
        => new(this);

    public override ItemFileQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        return this;
    }
}
