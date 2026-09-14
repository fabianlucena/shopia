using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IServices;
using backend_shopia.QueryOptions;
using Microsoft.AspNetCore.Mvc;
using RFBase.ILibs;
using RFBase.Libs;
using RFPermissions.Attributes;
using System.Globalization;
using System.Text.Json;

namespace backend_shopia.Controllers
{
    [ApiController]
    [Route("v1/item")]
    public class ItemController(
        ILogger<ItemController> logger,
        IItemService itemService,
        IItemFileService itemFileService,
        IItemStoreService itemStoreService,
        ICommerceService commerceService,
        IServiceProvider serviceProvider
    )
        : ControllerBase
    {
        [HttpPost]
        [Permission("item.add")]
        public async Task<IActionResult> PostAsync([FromBody] ItemAddRequest data)
        {
            logger.LogInformation("Creating item");

            if (data.CommerceUuid == default)
                throw new NoCommerceException();

            if (data.CategoryUuid == default)
                throw new NoCategoryException();

            if (data.StoresUuid == default
                || data.StoresUuid.Length <= 0
                || data.StoresUuid.Any(s => s == default)
            )
                throw new NoStoreProvidedException();

            if (data.Price < 0)
                return BadRequest("Price cannot be negative.");

            await commerceService.CheckByUuidAndCurrentUserAsync(data.CommerceUuid);

            var item = await data.ToItemAsync(serviceProvider);

            var result = await itemService.CreateAsync(item);
            if (result == null)
                return BadRequest();

            var updateImagesResult = await UpdateImages(result.Id, null);
            if (updateImagesResult is BadRequestObjectResult)
                return updateImagesResult;

            logger.LogInformation("Item created");

            return Ok();
        }

        [HttpGet("{uuid?}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid? uuid)
        {
            logger.LogInformation("Getting items");

            var options = new ItemQueryOptions()
                .UpdateFromRequest(HttpContext.Request);
            options.Uuid = uuid;

            options.InheritedIsActive = true;
            options.IncludeCategory = true;
            options.IncludeCommerce = true;

            var itemsList = await itemService.GetListAsync(options);

            var response = itemsList.Select(i => new ItemResponse(i));

            if (response.Any())
            {
                HashSet<Guid> commercesUuidList;
                if (await itemService.GetCurrentUserIdOrDefaultAsync() is not null)
                {
                    commercesUuidList = [.. (await commerceService.GetListUuidByCurrentUserAsync())];
                } else
                {
                    commercesUuidList = [];
                }

                var itemIdMap = itemsList.ToDictionary(i => i.Uuid, i => i.Id);

                response = await Task.WhenAll(response.Select(async item =>
                {
                    item.IsMine = commercesUuidList.Contains(item.Commerce!.Uuid);
                    if (itemIdMap.TryGetValue(item.Uuid, out var itemId))
                    {
                        var files = await itemFileService.GetListByItemIdAsync(itemId);
                        item.Images = [.. files.Select(f => new ItemImageDTO {
                            Uuid = f.Uuid,
                            Url = $"/v1/item/image/{f.Uuid}",
                        }).ToList()];

                        var storesList = await itemStoreService.GetStoresByItemIdAsync(itemId);
                        item.Stores = [..storesList.Select(s => new StoreMinimalDTO(s))];
                        item.StoresUuid = [..item.Stores.Select(s => s.Uuid)];
                    }

                    return item;
                }));
            }

            logger.LogInformation("Items retrieved");

            return Ok(response);
        }

        [HttpPatch("{uuid}")]
        [Permission("item.edit")]
        public async Task<IActionResult> PatchAsync([FromRoute] Guid uuid)
        {
            logger.LogInformation("Updating item");

            await itemService.CheckByUuidAndCurrentUserAsync(uuid);

            IDataDictionary data;
            List<Guid>? deletedImages = null;
            if (Request.HasFormContentType)
            {
                data = new DataDictionary();
                var formData = Request.Form;
                foreach (var key in formData.Keys)
                    data[key] = formData[key];

                var deletedImagesStrings = Request.Form["deletedImages"];
                if (deletedImagesStrings.Count > 1)
                    return BadRequest("Error multiple deleted images objects.");

                if (deletedImagesStrings.Count == 1)
                    deletedImages = JsonSerializer.Deserialize<List<Guid>>(deletedImagesStrings[0]!);
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                string bodyContent = await reader.ReadToEndAsync();
                data = JsonSerializer.Deserialize<DataDictionary>(bodyContent)!
                    .GetPascalized();

                if (data.TryGetGuids("DeletedImages", out var deletedImagesEnumerable) 
                    && deletedImagesEnumerable != null)
                {
                    deletedImages = [.. deletedImagesEnumerable];
                }
            }

            if (data.TryGetValue("Price", out object? value) && value is string priceText)
                data["Price"] = decimal.Parse(priceText, CultureInfo.InvariantCulture);

            var result = await itemService.UpdateByUuidAsync(uuid, data);
            if (result <= 0)
                return BadRequest();

            var id = await itemService.GetSingleIdByUuidAsync(
                uuid,
                new ItemQueryOptions { IncludeInactive = true }
            );

            var updateImagesResult = await UpdateImages(id, deletedImages);
            if (updateImagesResult is BadRequestObjectResult)
                return updateImagesResult;

            if (data.ContainsKey("IsActive"))
                _ = await itemService.UpdateInheritedByUuidAsync(uuid);

            logger.LogInformation("Item updated");

            return Ok();
        }

        private async Task<IActionResult> UpdateImages(long itemId, List<Guid>? deletedImages)
        {
            if (Request.HasFormContentType)
            {
                if (Request.Form.Files.Count > 0)
                {
                    var files = new FilesCollectionDTO(Request.Form.Files);
                    foreach (var file in Request.Form.Files)
                    {
                        if (file.Length == 0)
                            continue;

                        if (!file.ContentType.StartsWith("image/"))
                            return BadRequest("Only image files are allowed.");
                    }

                    var result = await itemFileService.AddByItemIdAsync(itemId, files);
                    if (!result.Any())
                        return BadRequest("Error uploading image.");
                }
            }

            if (deletedImages != null)
            {
                foreach (var uuid in deletedImages)
                {
                    var result = await itemFileService.DeleteByUuidAsync(uuid);
                    if (result <= 0)
                        return BadRequest();
                }
            }

            return Ok();
        }

        [HttpGet("image/{uuid}")]
        public async Task<IActionResult> GetImageAsync([FromRoute] Guid uuid)
        {
            logger.LogInformation("Getting item image for UUID: {Uuid}", uuid);

            var file = await itemFileService.GetSingleOrDefaultByUuidAsync(uuid)
                ?? throw new ItemImageNotFoundException();

            logger.LogInformation("Item image retrieved for UUID: {Uuid}", uuid);

            return File(file.Content, file.ContentType);
        }
    }
}
