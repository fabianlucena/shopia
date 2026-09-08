using RFIServices.QueryOptions;

namespace backend_shopia.QueryOptions;

public class StoreQueryOptions : ANominableOwnedEntityQueryOptions
{
    public bool IncludeCommerce { get; set; }

    public StoreQueryOptions() { }

    public StoreQueryOptions(StoreQueryOptions? options)
        : base(options)
    {
        if (options is null)
            return;

        IncludeCommerce = options.IncludeCommerce;
    }

    public override StoreQueryOptions Clone()
        => new(this);

    public override StoreQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        IncludeCommerce = GetBoolFromRequest(request, "includeCommerce", IncludeCommerce);

        return this;
    }
}
