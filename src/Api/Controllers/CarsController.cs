using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorsportApi.Infrastructure;
using MotorsportApi.Domain.Entities;
using AutoMapper;
using MotorsportApi.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace MotorsportApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;


    public CarsController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/cars
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cars = await _context.Cars
            .Include(c => c.Driver)
            .ToListAsync();
        var carDtos = _mapper.Map<List<CarDto>>(cars);

        return Ok(carDtos);
    }

    // GET: api/cars/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var car = await _context.Cars
            .Include(c => c.Driver)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (car == null) return NotFound();

        var carDto = _mapper.Map<CarDto>(car);

        return Ok(carDto);
    }

    // POST: api/cars
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CarInputDto carDto)
    {

        var existingCar = await _context.Cars.FirstOrDefaultAsync(c => c.DriverId == carDto.DriverId);
        if (existingCar != null) return BadRequest("This driver already has a car.");

        var car = _mapper.Map<Car>(carDto);

        _context.Cars.Add(car);
        await _context.SaveChangesAsync();

        var createdDto = _mapper.Map<CarDto>(car);
        return CreatedAtAction(nameof(GetById), new { id = car.Id }, createdDto);
    }

    // PUT: api/cars/5
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CarInputDto updatedDto)
    {
        var existingCar = await _context.Cars.FirstOrDefaultAsync(c => c.DriverId == updatedDto.DriverId);
        if (existingCar != null) return BadRequest($"This driver already has a car.");

        var car = await _context.Cars.FindAsync(id);
        if (car == null) return NotFound();

        _mapper.Map(updatedDto, car);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/cars/5
    [Authorize(Roles = "Admin")]
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
