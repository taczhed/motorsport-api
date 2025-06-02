using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorsportApi.Application.DTOs;
using MotorsportApi.Domain.Entities;
using MotorsportApi.Infrastructure;

namespace MotorsportApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RacesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RacesController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/races
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var races = await _context.Races
            .Include(r => r.Track)
            .Include(r => r.DriverRaces)
            .ThenInclude(dr => dr.Driver)
            .ToListAsync();

        var raceDtos = _mapper.Map<List<RaceDto>>(races);

        return Ok(raceDtos);
    }

    // GET: api/races/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var race = await _context.Races
            .Include(r => r.Track)
            .Include(r => r.DriverRaces)
            .ThenInclude(dr => dr.Driver)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (race == null) return NotFound();

        var raceDto = _mapper.Map<RaceDto>(race);

        return Ok(raceDto);
    }

    // POST: api/races
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RaceDto raceDto)
    {
        var trackExists = await _context.Tracks.AnyAsync(t => t.Id == raceDto.Track.Id);
        if (!trackExists) return BadRequest($"Track with ID {raceDto.Track.Id} does not exist.");

        var race = _mapper.Map<Race>(raceDto);
        _context.Races.Add(race);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = race.Id }, _mapper.Map<RaceDto>(race));
    }

    // PUT: api/races/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RaceDto updatedDto)
    {
        var race = await _context.Races.FindAsync(id);
        if (race == null) return NotFound();

        _mapper.Map(updatedDto, race);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/races/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var race = await _context.Races.FindAsync(id);
        if (race == null) return NotFound();

        _context.Races.Remove(race);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/races/{raceId}/drivers
    [HttpPost("{raceId}/drivers")]
    public async Task<IActionResult> AddDriverToRace(int raceId, [FromBody] DriverRaceDto driverRaceDto)
    {
        if (!await _context.Races.AnyAsync(r => r.Id == raceId))
            return NotFound($"Race with ID {raceId} not found.");

        if (!await _context.Drivers.AnyAsync(d => d.Id == driverRaceDto.Id)) 
            return BadRequest($"Driver with ID {driverRaceDto.Id} does not exist.");

        // Unikalność: tylko jeden wpis na kierowcę i wyścig
        var exists = await _context.DriverRaces.AnyAsync(dr => 
            dr.RaceId == raceId && dr.DriverId == driverRaceDto.Id);
        if (exists) return Conflict("Driver already assigned to this race.");

        var entity = _mapper.Map<DriverRace>(driverRaceDto);
        entity.RaceId = raceId;

        _context.DriverRaces.Add(entity);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<DriverRaceDto>(entity));
    }

    // PUT: api/races/{raceId}/drivers/{driverId}
    [HttpPut("{raceId}/drivers/{driverId}")]
    public async Task<IActionResult> UpdateDriverResult(int raceId, int driverId, [FromBody] DriverRaceDto updateDto)
    {
        var record = await _context.DriverRaces.FirstOrDefaultAsync(dr => dr.RaceId == raceId && dr.DriverId == driverId);

        if (record == null) return NotFound("Result not found for given driver and race.");

        record.Position = updateDto.Position.GetValueOrDefault();
        record.Time = updateDto.Time.GetValueOrDefault();

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/races/{raceId}/drivers/{driverId}
    [HttpDelete("{raceId}/drivers/{driverId}")]
    public async Task<IActionResult> RemoveDriverFromRace(int raceId, int driverId)
    {
        var record = await _context.DriverRaces.FirstOrDefaultAsync(dr => dr.RaceId == raceId && dr.DriverId == driverId);

        if (record == null) return NotFound("Driver is not assigned to this race.");

        _context.DriverRaces.Remove(record);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}
