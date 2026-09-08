using RFIServices.QueryOptions;

namespace backend_shopia.QueryOptions;

public class ItemQueryOptions : NominableEntityQueryOptions
{
    public ItemQueryOptions() { }

    public ItemQueryOptions(ItemQueryOptions? options)
        : base(options)
    {
        if (options is null)
            return;
    }

    public override ItemQueryOptions Clone()
        => new(this);

    public override ItemQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        return this;
    }
}
