using backend_shopia.DTO;
using backend_shopia.Exceptions;
using backend_shopia.IServices;
using backend_shopia.QueryOptions;
using Microsoft.AspNetCore.Mvc;
using RFBase.Libs;
using RFPermissions.Attributes;

namespace backend_shopia.Controllers;

[ApiController]
[Route("v1/store")]
public class StoreController(
    ILogger<StoreController> logger,
    IStoreService storeService,
    ICommerceService commerceService,
    IItemService itemService,
    IServiceProvider serviceProvider
)
    : ControllerBase
{
    [HttpPost]
    [Permission("store.add")]
    public async Task<IActionResult> PostAsync([FromBody] StoreAddRequest data)
    {
        logger.LogInformation("Creating store");

        if (data.CommerceUuid == default)
            throw new NoCommerceException();

        var store = await data.ToStoreAsync(serviceProvider);

        var commercesIdList = await commerceService.GetListIdForCurrentUserAsync();
        if (!commercesIdList.Contains(store.CommerceId))
            throw new CommerceDoesNotExistException();

        var result = await storeService.CreateAsync(store);

        if (result == null)
            return BadRequest();

        logger.LogInformation("Store created");

        return Ok();
    }

    [HttpGet("{uuid?}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid? uuid)
    {
        logger.LogInformation("Getting stores");

        var options = new StoreQueryOptions()
            .UpdateFromRequest(HttpContext.Request);

        if (uuid != null)
            options.Uuid = uuid;

        var storeList = await storeService.GetListAsync(options);

        var response = storeList.Select(s => new StoreResponse(s));

        logger.LogInformation("Stores retrieved");

        return Ok(response);
    }

    [HttpPatch("{uuid}")]
    [Permission("store.edit")]
    public async Task<IActionResult> PatchAsync([FromRoute] Guid uuid, [FromBody] DataDictionary data)
    {
        logger.LogInformation("Updating store");

        await storeService.CheckForUuidAndCurrentUserAsync(uuid);

        var result = await storeService.UpdateByUuidAsync(uuid, data.GetPascalized());

        if (result <= 0)
            return BadRequest();

        _ = await itemService.UpdateInheritedForStoreUuid(uuid);

        logger.LogInformation("Busines updated");

        return Ok();
    }

    [HttpDelete("{uuid}")]
    [Permission("store.delete")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid uuid)
    {
        logger.LogInformation("Deleting store");

        await storeService.CheckForUuidAndCurrentUserAsync(uuid);

        var result = await storeService.DeleteByUuidAsync(uuid);

        if (result <= 0)
            return BadRequest();

        _ = await itemService.UpdateInheritedForStoreUuid(uuid);

        logger.LogInformation("Store deleted");

        return Ok();
    }

    [HttpPost("restore/{uuid}")]
    [Permission("store.restore")]
    public async Task<IActionResult> RestoreAsync([FromRoute] Guid uuid)
    {
        logger.LogInformation("Restoring store");

        await storeService.CheckForUuidAndCurrentUserAsync(
            uuid,
            new StoreQueryOptions { IncludeDeleted = true }
        );

        var result = await storeService.RestoreByUuidAsync(uuid);

        if (result <= 0)
            return BadRequest();

        _ = await itemService.UpdateInheritedForStoreUuid(uuid);

        logger.LogInformation("Store restored");

        return Ok();
    }
}
