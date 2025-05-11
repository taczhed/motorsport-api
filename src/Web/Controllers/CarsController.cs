using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorsportApi.Infrastructure;
using MotorsportApi.Domain.Entities;

namespace MotorsportApi.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CarsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/cars
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cars = await _context.Cars
            .Include(c => c.Driver)
            .ToListAsync();

        return Ok(cars);
    }

    // GET: api/cars/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var car = await _context.Cars
            .Include(c => c.Driver)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (car == null) return NotFound();

        return Ok(car);
    }

    // POST: api/cars
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Car car)
    {
  
        var existingCar = await _context.Cars.FirstOrDefaultAsync(c => c.DriverId == car.DriverId);
        if (existingCar != null) return BadRequest("This driver already has a car.");

        _context.Cars.Add(car);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = car.Id }, car);
    }

    // PUT: api/cars/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Car updated)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car == null) return NotFound();

        car.Brand = updated.Brand;
        car.Model = updated.Model;
        car.Number = updated.Number;
        car.DriverId = updated.DriverId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/cars/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car == null) return NotFound();

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
