using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MotorsportApi.Domain.Entities;
using MotorsportApi.Infrastructure;

namespace MotorsportApi.Web.Pages.Races
{
    [Authorize(Roles = "Admin,RaceManager")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Race Race { get; set; } = default!;

        public List<DriverRace> DriverRaces { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var race = await _context.Races
                .Include(r => r.Track)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (race == null) return NotFound();

            Race = race;

            DriverRaces = await _context.DriverRaces
                .Where(dr => dr.RaceId == id)
                .Include(dr => dr.Driver)
                .ToListAsync();

            ViewData["TrackId"] = new SelectList(_context.Tracks, "Id", "Location");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            _context.Attach(Race).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RaceExists(Race.Id)) return NotFound();
                else throw;
            }

            return RedirectToPage("./Index");
        }

        private bool RaceExists(int id)
        {
            return _context.Races.Any(e => e.Id == id);
        }
    }
}
