using RFIServices.QueryOptions;

namespace backend_shopia.QueryOptions;

public class ItemStoreQueryOptions : ANominableOwnedEntityQueryOptions
{
    public ItemStoreQueryOptions() { }

    public ItemStoreQueryOptions(ItemStoreQueryOptions? options)
        : base(options)
    {
        if (options is null)
            return;
    }

    public override ItemStoreQueryOptions Clone()
        => new(this);

    public override ItemStoreQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        return this;
    }
}
