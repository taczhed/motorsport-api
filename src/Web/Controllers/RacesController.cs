using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorsportApi.Domain.Entities;
using MotorsportApi.Infrastructure;

namespace MotorsportApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RacesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RacesController(ApplicationDbContext context)
    {
        _context = context;
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

        return Ok(races);
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

        return Ok(race);
    }

    // POST: api/races
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Race race)
    {
        var trackExists = await _context.Tracks.AnyAsync(t => t.Id == race.TrackId);
        if (!trackExists) return BadRequest($"Track with ID {race.TrackId} does not exist.");

        _context.Races.Add(race);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = race.Id }, race);
    }

    // PUT: api/races/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Race updated)
    {
        var race = await _context.Races.FindAsync(id);
        if (race == null) return NotFound();

        race.Name = updated.Name;
        race.Date = updated.Date;
        race.TrackId = updated.TrackId;

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
    public async Task<IActionResult> AddDriverToRace(int raceId, [FromBody] DriverRace driverRace)
    {
        var race = await _context.Races.FindAsync(raceId);
        if (race == null) return NotFound($"Race {raceId} not found.");

        var driverExists = await _context.Drivers.AnyAsync(d => d.Id == driverRace.DriverId);
        if (!driverExists) return BadRequest($"Driver {driverRace.DriverId} does not exist.");

        // Unikalność: tylko jeden wpis na kierowcę i wyścig
        var exists = await _context.DriverRaces.AnyAsync(dr =>
            dr.RaceId == raceId && dr.DriverId == driverRace.DriverId);
        if (exists) return Conflict("Driver already assigned to this race.");

        driverRace.RaceId = raceId;

        _context.DriverRaces.Add(driverRace);
        await _context.SaveChangesAsync();

        return Ok(driverRace);
    }

    // PUT: api/races/{raceId}/drivers/{driverId}
    [HttpPut("{raceId}/drivers/{driverId}")]
    public async Task<IActionResult> UpdateDriverResult(int raceId, int driverId, [FromBody] DriverRace update)
    {
        var record = await _context.DriverRaces.FirstOrDefaultAsync(dr => dr.RaceId == raceId && dr.DriverId == driverId);

        if (record == null) return NotFound("Result not found for given driver and race.");

        record.Position = update.Position;
        record.Time = update.Time;

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
