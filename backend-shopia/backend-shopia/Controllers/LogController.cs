using Microsoft.AspNetCore.Mvc;
using RFLoggerProvider.DTO;
using RFLoggerProvider.IServices;
using RFLoggerProvider.QueryOptions;
using RFPermissions.Attributes;

namespace backend_shopia.Controllers;

[ApiController]
[Route("v1/log")]
public class LogController(
    ILogger<LogController> logger,
    ILogService logService
) : ControllerBase
{
    [HttpGet]
    [Permission("log.get")]
    public async Task<IActionResult> GetAsync()
    {
        logger.LogInformation("Getting log");

        var options = new LogQueryOptions
        {
            IncludeLevel = true,
            IncludeAction = true,
            IncludeSession = true,
            IncludeProject = true,
            IncludeUser = true,
        }.UpdateFromRequest(HttpContext.Request);

        var result = (await logService.GetListAsync(options))
            .Select(l => new LogResponse(l));

        return Ok(result);
    }
}
