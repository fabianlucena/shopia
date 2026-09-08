using backend_shopia.DTO;
using backend_shopia.IServices;
using backend_shopia.QueryOptions;
using Microsoft.AspNetCore.Mvc;
using RFPermissions.Attributes;

namespace backend_shopia.Controllers;

[ApiController]
[Route("v1/category")]
public class CategoryController(
    ILogger<CategoryController> logger,
    ICategoryService categoryService
)
    : ControllerBase
{
    [HttpGet("{uuid?}")]
    [Permission("category.get")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid? uuid)
    {
        logger.LogInformation("Getting categories");

        var options = new CategoryQueryOptions().UpdateFromRequest(HttpContext.Request);
        if (uuid != null)
            options.Uuid = uuid;

        var categoriesList = await categoryService.GetListAsync(options);

        var response = categoriesList.Select(c => new CategoryResponse(c));

        logger.LogInformation("Categories retrieved");

        return Ok(response);
    }
}
