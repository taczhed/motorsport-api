using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorsportApi.Api.Middleware;

namespace MotorsportApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatsController : ControllerBase
{
    // GET: api/stats
    [Authorize(Roles = "Admin")]
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