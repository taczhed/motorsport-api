using Microsoft.AspNetCore.Mvc;
using MotorsportApi.Web.Middleware;

namespace MotorsportApi.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatsController : ControllerBase
{
    // GET: api/stats
    [HttpGet]
    public IActionResult GetStats()
    {
        var stats = new
        {
            TotalRequests = RequestCountingMiddleware.GetRequestCount()
        };

        return Ok(stats);
    }
}