using RFIServices.QueryOptions;

namespace backend_shopia.QueryOptions;

public class ItemQueryOptions : ANominableEntityQueryOptions
{
    public bool IncludeStores { get; set; }
    public bool IncludeCommerce { get; set; }

    public IEnumerable<long>? CommercesId { get; set; }
    public Guid? StoreUuid { get; set; }
    public Guid? CommerceUuid { get; set; }

    public ItemQueryOptions() { }

    public ItemQueryOptions(ItemQueryOptions? options)
        : base(options)
    {
        if (options is null)
            return;

        IncludeStores = options.IncludeStores;
        IncludeCommerce = options.IncludeCommerce;

        CommercesId = options.CommercesId;
        StoreUuid = options.StoreUuid;
        CommerceUuid = options.CommerceUuid;
    }

    public override ItemQueryOptions Clone()
        => new(this);

    public override ItemQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        return this;
    }
}
