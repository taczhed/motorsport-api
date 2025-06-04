using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorsportApi.Infrastructure;
using MotorsportApi.Domain.Entities;
using AutoMapper;
using MotorsportApi.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace MotorsportApi.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DriversController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/drivers
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var drivers = await _context.Drivers
            .Include(d => d.Car)
            .Include(d => d.DriverRaces)
            .ThenInclude(dr => dr.Race)
            .ToListAsync();

        var driverDtos = _mapper.Map<List<DriverDto>>(drivers);
        return Ok(driverDtos);
    }

    // GET: api/drivers/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var driver = await _context.Drivers
            .Include(d => d.Car)
            .Include(d => d.DriverRaces)
            .ThenInclude(dr => dr.Race)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (driver == null) return NotFound();

        var driverDto = _mapper.Map<DriverDto>(driver);
        return Ok(driverDto);
    }

    // POST: api/drivers
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DriverInputDto driverDto)
    {
        var driver = _mapper.Map<Driver>(driverDto);
        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = driver.Id }, _mapper.Map<DriverDto>(driver));
    }

    // PUT: api/drivers/5
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DriverInputDto updatedDto)
    {
        var driver = await _context.Drivers.FindAsync(id);
        if (driver == null) return NotFound();

        _mapper.Map(updatedDto, driver);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/drivers/5
    [Authorize(Roles = "Admin")]
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
