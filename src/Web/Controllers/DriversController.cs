using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorsportApi.Infrastructure;
using MotorsportApi.Domain.Entities;

namespace MotorsportApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DriversController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/drivers
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var drivers = await _context.Drivers
            .Include(d => d.Car)
            .Include(d => d.DriverRaces)
            .ToListAsync();

        return Ok(drivers);
    }

    // GET: api/drivers/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var driver = await _context.Drivers
            .Include(d => d.Car)
            .Include(d => d.DriverRaces)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (driver == null) return NotFound();

        return Ok(driver);
    }

    // POST: api/drivers
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Driver driver)
    {
        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = driver.Id }, driver);
    }

    // PUT: api/drivers/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Driver updated)
    {
        var driver = await _context.Drivers.FindAsync(id);
        if (driver == null) return NotFound();

        driver.Name = updated.Name;
        driver.Age = updated.Age;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/drivers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var driver = await _context.Drivers.FindAsync(id);
        if (driver == null) return NotFound();

        _context.Drivers.Remove(driver);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
