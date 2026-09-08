using RFIServices.QueryOptions;

namespace backend_shopia.QueryOptions;

public class CommerceQueryOptions : ANominableEntityQueryOptions
{
    public bool IncludeStores { get; set; }

    public CommerceQueryOptions() { }

    public CommerceQueryOptions(CommerceQueryOptions? options)
        : base(options)
    {
        if (options is null)
            return;

        IncludeStores = options.IncludeStores;
    }

    public override CommerceQueryOptions Clone()
        => new(this);

    public override CommerceQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        IncludeStores = GetBoolFromRequest(request, "includeStores", IncludeStores);

        return this;
    }
}
