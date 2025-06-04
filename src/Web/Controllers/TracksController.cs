using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorsportApi.Application.DTOs;
using MotorsportApi.Domain.Entities;
using MotorsportApi.Infrastructure;

namespace MotorsportApi.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TracksController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public TracksController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/tracks
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tracks = await _context.Tracks
            .Include(t => t.Races)
            .ToListAsync();
        
        var trackDtos = _mapper.Map<List<TrackDto>>(tracks);

        return Ok(trackDtos);
    }

    // GET: api/tracks/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var track = await _context.Tracks
            .Include(t => t.Races)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (track == null) return NotFound();

        var trackDto = _mapper.Map<TrackDto>(track);

        return Ok(trackDto);
    }

    // POST: api/tracks
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TrackInputDto trackDto)
    {
        var track = _mapper.Map<Track>(trackDto);
        _context.Tracks.Add(track);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = track.Id }, _mapper.Map<TrackDto>(track));
    }

    // PUT: api/tracks/5
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TrackInputDto updatedDto)
    {
        var track = await _context.Tracks.FindAsync(id);
        if (track == null) return NotFound();

        _mapper.Map(updatedDto, track);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/tracks/5
    [Authorize(Roles = "Admin")]
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
