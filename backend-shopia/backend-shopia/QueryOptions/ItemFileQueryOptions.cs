using RFIServices.QueryOptions;

namespace backend_shopia.QueryOptions;

public class ItemFileQueryOptions : ACreatableWithNameEntityQueryOptions
{
    public bool JoinCommerce { get; set; }

    public long? ItemId { get; set; }
    public IEnumerable<long>? ItemsId { get; set; }


    public ItemFileQueryOptions() { }

    public ItemFileQueryOptions(ItemFileQueryOptions? options)
        : base(options)
    {
        if (options is null)
            return;

        JoinCommerce = options.JoinCommerce;

        ItemId = options.ItemId;
        ItemsId = options.ItemsId;
    }

    public override ItemFileQueryOptions Clone()
        => new(this);

    public override ItemFileQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        return this;
    }
}
