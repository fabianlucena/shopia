using backend_shopia.Entities;
using backend_shopia.IRepositories;
using backend_shopia.QueryOptions;
using Microsoft.EntityFrameworkCore;
using RFEntitiesEF.Repositories;
using RFIServices.IServices;
using RFIServices.QueryOptions;
using RFRegisterService.Attributes;
using System.Linq.Expressions;

namespace backend_shopia.Repositories;

[RegisterService]
public class ItemRepository(
    DbContext context,
    IServiceProvider serviceProvider
)
    : ANominableEntityRepository<Item>(context),
    IItemRepository
{
    public override IQueryable<Item> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        if (options is ItemQueryOptions itemOptions)
        {
            if (itemOptions.IncludeCategory)
                queryable = queryable.Include(i => i.Category);

            if (itemOptions.IncludeCommerce || itemOptions.CommerceUuid is not null || itemOptions.Mine is not null)
                queryable = queryable.Include(i => i.Commerce);

            if (itemOptions.CommercesId is not null)
                queryable = queryable.Where(i => itemOptions.CommercesId.Contains(i.CommerceId));

            if (itemOptions.StoreUuid is not null)
                queryable = queryable
                    .Include(i => i.Commerce)
                    .Where(i => i.Stores!.Any(s => s.Uuid == itemOptions.StoreUuid));

            if (itemOptions.CommerceUuid is not null)
                queryable = queryable.Where(i => i.Commerce!.Uuid == itemOptions.CommerceUuid);

            if (itemOptions.InheritedIsActive is not null)
                queryable = queryable.Where(i => i.InheritedIsActive == itemOptions.InheritedIsActive);

            if (itemOptions.Mine is not null)
            {
                var userService = serviceProvider.GetRequiredService<IUserService>();
                var userId = userService.GetCurrentUserIdAsync().Result;
                if (itemOptions.Mine.Value)
                    queryable = queryable.Where(i => i.Commerce!.OwnerId == userId);
                else
                    queryable = queryable.Where(i => i.Commerce!.OwnerId != userId);
            }
        }

        return queryable;
    }

    private async Task<int> UpdateInheritedAsync(Expression<Func<Item, bool>> itemFilter)
    {
        var context = Context as AppDbContext;
        return await context!.Items
            .Where(itemFilter)
            .ExecuteUpdateAsync(s => s.SetProperty(
                i => i.InheritedIsActive,
                i => i.IsActive && i.DeletedAt == null &&
                    context.Commerces
                        .Where(c => c.Id == i.CommerceId && c.IsActive && c.DeletedAt == null)
                        .Any() &&
                    context.ItemsStores
                        .Where(t => t.ItemId == i.Id)
                        .Join(
                            context.Stores,
                            t => t.StoreId,
                            s => s.Id,
                            (t, store) => store
                        )
                        .Any(store => store.IsActive && store.DeletedAt == null)
            ));
    }

    public async Task<int> UpdateInheritedByUuidAsync(Guid uuid)
    {
        var context = Context as AppDbContext;
        return await UpdateInheritedAsync(i => i.Uuid == uuid);
    }

    public async Task<int> UpdateInheritedByCommerceUuidAsync(Guid commerceUuid)
    {
        var context = Context as AppDbContext;
        return await UpdateInheritedAsync(i =>
            context!.Commerces.Any(c => c.Id == i.CommerceId && c.Uuid == commerceUuid && c.IsActive && c.DeletedAt == null));
    }

    public async Task<int> UpdateInheritedByStoreUuidAsync(Guid storeUuid)
    {
        var context = Context as AppDbContext;
        return await UpdateInheritedAsync(i =>
            context!.ItemsStores.Any(t => t.ItemId == i.Id && t.Store!.Uuid == storeUuid && t.Store.IsActive && t.Store.DeletedAt == null));
    }
}
