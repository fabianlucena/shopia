using RFIServices.QueryOptions;

namespace backend_shopia.QueryOptions;

public class ItemStoreQueryOptions : ANominableOwnedEntityQueryOptions
{
    public bool IncludeItem { get; set; }
    public bool IncludeStore { get; set; }

    public long? ItemId { get; set; }
    public long? StoreId { get; set; }
    public IEnumerable<long>? ItemsId { get; set; }
    public IEnumerable<long>? StoresId { get; set; }

    public ItemStoreQueryOptions() { }

    public ItemStoreQueryOptions(ItemStoreQueryOptions? options)
        : base(options)
    {
        if (options is null)
            return;

        IncludeItem = options.IncludeItem;
        IncludeStore = options.IncludeStore;

        ItemId = options.ItemId;
        StoreId = options.StoreId;
        ItemsId = options.ItemsId;
        StoresId = options.StoresId;
    }

    public override ItemStoreQueryOptions Clone()
        => new(this);

    public override ItemStoreQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        return this;
    }
}
