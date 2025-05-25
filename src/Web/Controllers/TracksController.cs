using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorsportApi.Domain.Entities;
using MotorsportApi.Infrastructure;

namespace MotorsportApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TracksController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TracksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/tracks
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tracks = await _context.Tracks
            .Include(t => t.Races)
            .ToListAsync();

        return Ok(tracks);
    }

    // GET: api/tracks/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var track = await _context.Tracks
            .Include(t => t.Races)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (track == null) return NotFound();

        return Ok(track);
    }

    // POST: api/tracks
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Track track)
    {
        _context.Tracks.Add(track);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = track.Id }, track);
    }

    // PUT: api/tracks/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Track updated)
    {
        var track = await _context.Tracks.FindAsync(id);
        if (track == null) return NotFound();

        track.Name = updated.Name;
        track.Location = updated.Location;
        track.LengthKm = updated.LengthKm;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/tracks/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var track = await _context.Tracks.FindAsync(id);
        if (track == null) return NotFound();

        _context.Tracks.Remove(track);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
