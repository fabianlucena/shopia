using backend_shopia.DTO;
using backend_shopia.IServices;
using Microsoft.AspNetCore.Mvc;
using RFService.Authorization;
using RFService.Data;

namespace backend_shopia.Controllers
{
    [ApiController]
    [Route("v1/my-plan")]
    public class MyPlanController(
        ILogger<MyPlanController> logger,
        IPlanService planService
    )
        : ControllerBase
    {
        [Permission("my-plan.get")]
        public async Task<IActionResult> GetAsync()
        {
            logger.LogInformation("Getting my plan");
            var myPlan = await planService.GetMyPlanAsync();
            logger.LogInformation("My plan retrieved");
            return Ok(new DataRowsResult(new List<MyPlanResponse> { myPlan }));
        }
    }
}
