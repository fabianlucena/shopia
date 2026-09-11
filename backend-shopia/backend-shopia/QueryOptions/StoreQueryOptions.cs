using RFIServices.QueryOptions;

namespace backend_shopia.QueryOptions;

public class StoreQueryOptions : ANominableOwnedEntityQueryOptions
{
    public bool IncludeCommerce { get; set; }
    public bool JoinCommerce { get; set; }

    public long? CommerceId { get; set; }
    public IEnumerable<long>? CommercesId { get; set; }
    public long? CommerceOwnerId { get; set; }

    public StoreQueryOptions() { }

    public StoreQueryOptions(StoreQueryOptions? options)
        : base(options)
    {
        if (options is null)
            return;

        IncludeCommerce = options.IncludeCommerce;
        JoinCommerce = options.JoinCommerce;

        CommerceId = options.CommerceId;
        CommercesId = options.CommercesId;
        CommerceOwnerId = options.CommerceOwnerId;
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
