using backend_shopia.Services;
using Microsoft.Extensions.Options;
using RFIServices.QueryOptions;

namespace backend_shopia.QueryOptions;

public class ItemQueryOptions : ANominableEntityQueryOptions
{
    public bool IncludeCategory { get; set; }
    public bool IncludeStores { get; set; }
    public bool IncludeCommerce { get; set; }

    public IEnumerable<long>? CommercesId { get; set; }
    public Guid? StoreUuid { get; set; }
    public Guid? CommerceUuid { get; set; }
    public bool? InheritIsActive { get; set; }
    public bool? Mine { get; set; }
    /*if (GetBoolFromRequest(request, "mine"))
    {
        var commercesId = await commerceService.GetListIdByCurrentUserAsync(new CommerceQueryOptions { IncludeInactive = true });
        options.AddFilter("CommerceId", commercesId);
    }*/

    public ItemQueryOptions() { }

    public ItemQueryOptions(ItemQueryOptions? options)
        : base(options)
    {
        if (options is null)
            return;

        IncludeCategory = options.IncludeCategory;
        IncludeStores = options.IncludeStores;
        IncludeCommerce = options.IncludeCommerce;

        CommercesId = options.CommercesId;
        StoreUuid = options.StoreUuid;
        CommerceUuid = options.CommerceUuid;
        InheritIsActive = options.InheritIsActive;
        Mine = options.Mine;
    }

    public override ItemQueryOptions Clone()
        => new(this);

    public override ItemQueryOptions UpdateFromRequest(HttpRequest request)
    {
        base.UpdateFromRequest(request);

        IncludeStores = GetBoolFromRequest(request, "includeStores");
        IncludeCommerce = GetBoolFromRequest(request, "includeCommerce");

        Mine = GetBoolFromRequest(request, "mine");
        CommerceUuid = GetNullableUuidFromRequest(request, "commerceUuid");

        return this;
    }
}
