using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IServices;
using RFOperators;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace backend_shopia.Services
{
    public class ItemStoreService(
        IRepo<ItemStore> repo
    )
        : ServiceCreatedAt<IRepo<ItemStore>, ItemStore>(repo),
            IItemStoreService
    {
        public override async Task<ItemStore> ValidateForCreationAsync(ItemStore data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (data.ItemId <= 0)
            {
                data.ItemId = data.Item?.Id ?? 0;
                if (data.ItemId <= 0)
                    throw new NoItemException();
            }

            if (data.StoreId <= 0)
            {
                data.StoreId = data.Store?.Id ?? 0;
                if (data.StoreId <= 0)
                    throw new NoStoreException();
            }

            return data;
        }

        public override async Task<IEnumerable<ItemStore>> GetListAsync(QueryOptions options)
        {
            if (options.Switches.TryGetValue("IncludeStores", out var includeStores)
                && includeStores)
            {
                options.Include("Store", "store");
                if (options.Switches.TryGetValue("IncludeCommerce", out var includeCommerce)
                    && includeCommerce)
                {
                    options.Join.Add(new From(
                        propertyName: "Commerce",
                        alias: "commerce",
                        type: JoinType.Inner,
                        on: Op.Eq("commerce.Id", Op.Column("store.CommerceId")),
                        entity: typeof(Commerce)
                    ));
                }
            }

            return await base.GetListAsync(options);
        }

        public async Task<IEnumerable<ItemStore>> GetListForItemIdAsync(Int64 itemId, QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            options.AddFilterIfNotExists("ItemId", itemId);
            return await GetListAsync(options);
        }

        public async Task<IEnumerable<Store>> GetListStoresForItemIdAsync(Int64 itemId, QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            options.IncludeIfNotExists("Store", "store");

            return (await GetListForItemIdAsync(itemId, options))
                .Select(i => i.Store!);
        }
    }
}