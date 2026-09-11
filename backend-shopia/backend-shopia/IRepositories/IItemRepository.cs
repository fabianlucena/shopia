using backend_shopia.Entities;
using backend_shopia.QueryOptions;
using RFIRepositories.IRepositories;

namespace backend_shopia.IRepositories;

public interface IItemRepository : IANominableEntityRepository<Item>
{
    Task<int> UpdateInheritedByUuidAsync(Guid uuid, ItemQueryOptions? options = null);
    Task<int> UpdateInheritedByStoreUuidAsync(Guid storeUuid, ItemQueryOptions? options = null);
    Task<int> UpdateInheritedByCommerceUuidAsync(Guid commerceUuid, ItemQueryOptions? options = null);
}



/*public (ItemQueryOptions, DataDictionary) GetOptionsByUpdateInherited(ItemQueryOptions? options = null)
{
    options = options?.Clone() ?? new();
    options.IncludeItemStore(
        "",
        "itemStore",
        entity: typeof(ItemStore),
        on: Op.Eq("itemStore.ItemId", Op.Column("Id"))
    );
    options.Include(
        "",
        "store",
        entity: typeof(Store),
        on: Op.Eq("store.Id", Op.Column("itemStore.StoreId"))
    );
    options.Include(
        "",
        "commerce",
        entity: typeof(Commerce),
        on: Op.Eq("commerce.Id", Op.Column("store.CommerceId"))
    );

    var data = new DataDictionary
    {
        { "InheritedIsEnabled",
            Op.And(
                Op.Eq("store.IsEnabled", true),
                Op.IsNull("store.DeletedAt"),
                Op.Eq("commerce.IsEnabled", true),
                Op.IsNull("commerce.DeletedAt")
            )
        },
    };

    return (options, data);
}* /

public async Task<int> UpdateInheritedByUuid(Guid uuid, ItemQueryOptions? options = null)
{
    (options, DataDictionary data) = GetOptionsByUpdateInherited(options);
    options.Uuid = uuid;

    return await itemRepository.UpdateInheritedAsync(data, options);
}

/*public async Task<int> UpdateInheritedByStoreUuid(Guid storeUuid, ItemQueryOptions? options = null)
{
    (options, DataDictionary data) = GetOptionsByUpdateInherited(options);
    options.StoreUuid = storeUuid;
    data["Location"] = Op.Column("store.Location");

    return await UpdateAsync(data, options);
}

public async Task<int> UpdateInheritedByCommerceUuid(Guid commerceUuid, ItemQueryOptions? options = null)
{
    (options, DataDictionary data) = GetOptionsByUpdateInherited(options);
    options.CommerceUuid = commerceUuid;

    return await UpdateAsync(data, options);
}*/