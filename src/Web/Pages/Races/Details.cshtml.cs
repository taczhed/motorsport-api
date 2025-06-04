using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MotorsportApi.Domain.Entities;
using MotorsportApi.Infrastructure;

namespace MotorsportApi.Web.Pages.Races
{
    [Authorize(Roles = "Admin,RaceManager")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Race Race { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = await _context.Races.FirstOrDefaultAsync(m => m.Id == id);
            if (race == null)
            {
                return NotFound();
            }

            Race = race;

            var track = await _context.Tracks.FirstOrDefaultAsync(m => m.Id == race.TrackId);
            ViewData["Track"] = track;

            var driverInfos = await _context.DriverRaces
                .Where(dr => dr.RaceId == race.Id)
                .Include(dr => dr.Driver)
                .Select(dr => new
                {
                    dr.Driver.Name,
                    dr.Position,
                    dr.Time
                })
                .ToListAsync();

            ViewData["Drivers"] = driverInfos;

            return Page();
        }
    }
}
